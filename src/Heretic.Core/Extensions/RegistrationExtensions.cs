using ErikTheCoder.Heretic.Contracts.Internal.Services;
using ErikTheCoder.Heretic.Core.Services;
using Microsoft.Extensions.DependencyInjection;


namespace ErikTheCoder.Heretic.Core.Extensions;


public static class RegistrationExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IObjectMapper, ObjectMapper>()
            .AddSingleton<IUserService, UserService>();

        return services;
    }
}