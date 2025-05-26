using ErikTheCoder.Contracts.Internal.Repositories;
using ErikTheCoder.Contracts.Internal.Services;
using ErikTheCoder.Contracts.Services;
using ErikTheCoder.Identity.Options;
using ErikTheCoder.Identity.Repositories;
using ErikTheCoder.Identity.Services;
using ErikTheCoder.Utilities.Random;
using Microsoft.Extensions.DependencyInjection;


namespace ErikTheCoder.Identity.Extensions;


public static class RegistrationExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, Action<IdentityOptions> configureOptions)
    {
        if (configureOptions != null) services.AddOptions<IdentityOptions>().Configure(configureOptions);

        services
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IPasswordManagerProvider, PasswordManagerProvider>()
            .AddSingleton<IPasswordManager, Pbkdf2Mod1PasswordManager>()
            .AddSingleton<IThreadsafeRandom, ThreadsafeCryptoRandom>()
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<IObjectMapper, ObjectMapper>();

        return services;
    }
}