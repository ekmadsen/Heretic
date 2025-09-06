using ErikTheCoder.Contracts.Internal;
using ErikTheCoder.Heretic.WebApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using OptionsFactory = Microsoft.Extensions.Options.Options;


namespace ErikTheCoder.Heretic.WebApi.Extensions;


public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, Action<AuthOptions> configureOptions)
    {
        services.AddOptions<AuthOptions>().Configure(configureOptions);
        var authOptions = new AuthOptions();
        configureOptions(authOptions);

        services
            .AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // Create security key.
                var rsa = RSA.Create(); // Don't dispose via using pattern because reference to rsa variable is held by securityKey variable below.
                // The public key validates the signed token.
                var publicSigningKey = new ReadOnlySpan<byte>(Convert.FromBase64String(authOptions.PublicSigningKey));
                rsa.ImportRSAPublicKey(publicSigningKey, out var bytesRead);
                if (bytesRead != publicSigningKey.Length) throw new Exception("Failed to import RSA public key.");
                var securityKey = new RsaSecurityKey(rsa) { KeyId = authOptions.KeyId.ToString() };

                // Configure JWT token validation.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = [authOptions.Issuer],
                    ValidateAudience = true,
                    ValidAudiences = [authOptions.Audience],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey
                };
            });

        services.AddSingleton(OptionsFactory.Create(authOptions));
        return services;
    }


    public static IServiceCollection AddPolicyBasedAuthorization(this IServiceCollection services)
    {
        // Define security policies.
        // Enforce policies on REST controller endpoints via [Authorize(Policy = "PolicyName")] attribute.
        services
            .AddAuthorizationBuilder()
            .AddPolicy(PolicyName.Read, policy => policy.RequireClaim(ClaimName.Scope, Scope.ApiRead))
            .AddPolicy(PolicyName.Write, policy => policy.RequireClaim(ClaimName.Scope, Scope.ApiWrite));

        return services;
    }
}