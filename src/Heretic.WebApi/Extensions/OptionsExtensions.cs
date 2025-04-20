using ErikTheCoder.Logging.Options;


namespace ErikTheCoder.Heretic.WebApi.Extensions;


public static class OptionsExtensions
{
    public static IServiceCollection AddHereticOptions(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<FileLoggerOptions>(configuration.GetSection("Logger"));
        services.Configure<LoggerOptions>(configuration.GetSection("Logger"));

        return services;
    }
}