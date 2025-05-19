using ErikTheCoder.Contracts.Internal.Services;
using ErikTheCoder.Data.Options;
using ErikTheCoder.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ErikTheCoder.Data.Extensions;


public static class RegistrationExtensions
{
    public static IServiceCollection AddSqlDatabase(this IServiceCollection services, DatabaseOptions options)
    {
        services.AddSingleton<IDatabase, SqlDatabase>(serviceProvider => new SqlDatabase(serviceProvider, OptionsFactory.Create(options)));
        return services;
    }
}