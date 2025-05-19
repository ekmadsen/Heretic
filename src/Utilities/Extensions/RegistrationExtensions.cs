using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OptionsFactory = Microsoft.Extensions.Options.Options;


namespace ErikTheCoder.Utilities.Extensions;


public static class RegistrationExtensions
{
    public static IServiceCollection Configure<T>(this IServiceCollection services, IConfiguration config, Action<T> configureOptions) where T : class
    {
        var configuration = config.Get<T>();
        configureOptions(configuration);
        var options = OptionsFactory.Create(configuration);
        services.AddSingleton(options);

        return services;
    }
}