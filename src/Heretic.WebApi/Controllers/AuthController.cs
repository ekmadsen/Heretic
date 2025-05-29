using System.Security.Cryptography;
using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Internal;
using ErikTheCoder.Contracts.Services;
using ErikTheCoder.Identity.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("auth")]
public class AuthController(IOptions<IdentityOptions> options, IUserService userService)
{
    [HttpGet]
    [Route("metadata")]
    [AllowAnonymous]
    public async Task<AuthMetadata> GetMetadata()
    {
        // Export public signing key.
        var rsa = RSA.Create();
        var publicSigningKey = new ReadOnlySpan<byte>(Convert.FromBase64String(options.Value.PublicSigningKey));
        rsa.ImportRSAPublicKey(publicSigningKey, out var bytesRead);
        if (bytesRead != publicSigningKey.Length) throw new Exception("Failed to import RSA public key.");
        var securityKey = new RsaSecurityKey(rsa.ExportParameters(false)) { KeyId = options.Value.KeyId.ToString() };

        return await Task.FromResult(new AuthMetadata
        {
            PublicSigningKey = new PublicSigningKey
            {
                Id = options.Value.KeyId,
                AsPkcs1Base64 = Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                AsPem = rsa.ExportRSAPublicKeyPem(),
                AsJsonWebKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(securityKey)
            }
        });
    }


    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var response = await userService.Login(request);
        return response.Success
            ? new OkObjectResult(response.Token)
            : new UnauthorizedResult();
    }


    [HttpPatch]
    [Route("users/{Id:int}/password")]
    [Authorize(Policy = PolicyName.Write)]
    public async Task UpdateUserPassword(UpdatePasswordRequest request)
    {
        // TODO: Verify authenticated user is an admin or is the user specified in the request DTO.
        await userService.UpdatePassword(request);
    }
}