using System.Security.Cryptography;
using ErikTheCoder.Contracts;
using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Dtos.Responses;
using ErikTheCoder.Contracts.Internal;
using ErikTheCoder.Contracts.Services;
using ErikTheCoder.Contracts.Internal.Repositories;
using ErikTheCoder.Contracts.Internal.Services;
using ErikTheCoder.Identity.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;


namespace ErikTheCoder.Identity.Services;


public class UserService(IOptions<IdentityOptions> options, IUserRepository repository, IPasswordManagerProvider passwordManagerProvider, IObjectMapper objectMapper) : IUserService
{
    private const string _signatureAlgorithm = SecurityAlgorithms.RsaSha512Signature;


    public IAsyncEnumerable<User> GetUsers()
    {
        var userEntities = repository.GetUsers();
        return objectMapper.ToDtos(userEntities);
    }


    public async Task<User> GetUser(GetUserRequest request)
    {
        var userEntity = await repository.GetUser(request);
        return objectMapper.ToDto(userEntity);
    }


    public async Task<LoginResponse> Login(LoginRequest request)
    {
        // Get user from database.
        var userEntity = await repository.GetUser(request);
        if (userEntity == null)
        {
            return new LoginResponse
            {
                Success = false,
                Status = LoginStatus.UserNotFound
            };
        }

        // Verify user's password.
        var passwordManager = passwordManagerProvider.Get(userEntity.PasswordManagerId);
        if (!passwordManager.Validate(request.Payload.Password, userEntity.Salt, userEntity.PasswordHash))
        {
            return new LoginResponse
            {
                Success = false,
                Status = LoginStatus.PasswordIncorrect
            };
        }

        // Get user's claims.
        var claims = new Dictionary<string, object>
        {
            ["sub"] = userEntity.Username,
            ["firstName"] = userEntity.FirstName,
            ["lastName"] = userEntity.LastName,
            ["email"] = userEntity.Email
        };

        var claimEntities = repository.GetClaims(userEntity.Id);
        await foreach (var claimEntity in claimEntities)
        {
            if (claims.TryGetValue(claimEntity.Name, out var value))
            {
                // Claim name already present in dictionary.
                if (value is string stringValue)
                {
                    // Elevate single value to a list of values.
                    claims[claimEntity.Name] = new List<string>([stringValue, claimEntity.Value]);
                }
                else
                {
                    // Add value to list of values.
                    ((List<string>)claims[claimEntity.Name]).Add(claimEntity.Value);
                }
            }
            else
            {
                // Claim name not present in dictionary.
                // Add single value.
                claims[claimEntity.Name] = claimEntity.Value;
            }
        }

        // Create security key.
        var rsa = RSA.Create();
        // The public key validates the signed token.
        var publicSigningKey = new ReadOnlySpan<byte>(Convert.FromBase64String(options.Value.PublicSigningKey));
        rsa.ImportRSAPublicKey(publicSigningKey, out var bytesRead);
        if (bytesRead != publicSigningKey.Length) throw new Exception("Failed to import RSA public key.");
        var securityKey = new RsaSecurityKey(rsa) { KeyId = options.Value.KeyId.ToString() };
        // The private key signs the token.
        var privateSigningKey = new ReadOnlySpan<byte>(Convert.FromBase64String(options.Value.PrivateSigningKey));
        rsa.ImportRSAPrivateKey(privateSigningKey, out bytesRead);
        if (bytesRead != privateSigningKey.Length) throw new Exception("Failed to import RSA private key.");

        // Create token descriptor.
        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = now + TimeSpan.FromMinutes(options.Value.TokenExpirationMinutes),
            Claims = claims,
            IncludeKeyIdInHeader = true,
            SigningCredentials = new SigningCredentials(securityKey, _signatureAlgorithm)
        };

        // Create token and sign with security key provided in token descriptor.
        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Update last login.
        userEntity.LastLogin = DateTime.UtcNow;
        await repository.UpdateUser(userEntity);

        return new LoginResponse
        {
            Success = true,
            Status = LoginStatus.Success,
            Token = token
        };
    }


    public async Task UpdatePassword(UpdatePasswordRequest request)
    {
        // Get user from database.
        var userEntity = await repository.GetUser(request);

        // Update password hash using latest password manager.
        var passwordManager = passwordManagerProvider.Get(PasswordManagerName.Latest);
        var (salt, hash) = passwordManager.Hash(request.Payload.Password);

        // Save user to database.
        userEntity.Salt = salt;
        userEntity.PasswordHash = hash;
        userEntity.PasswordManagerId = passwordManager.Id;
        userEntity.PasswordChanged = DateTime.UtcNow;

        await repository.UpdateUser(userEntity);
    }
}