using ErikTheCoder.Contracts.Internal.Repositories;
using ErikTheCoder.Contracts.Services;
using ErikTheCoder.Data.Extensions;
using ErikTheCoder.Data.Options;
using ErikTheCoder.Data.Services;
using ErikTheCoder.Heretic.Contracts.Internal;
using ErikTheCoder.Logging.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ErikTheCoder.Heretic.Data.Extensions;


public static class RegistrationExtensions
{
    private const string _dbSuffix = "Database";


    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddSqlDatabase(new DatabaseOptions
            {
                Name = DatabaseName.Heretic,
                Connection = configuration.GetConnectionString($"{DatabaseName.Heretic}{_dbSuffix}"),
                LogQueries = true
            })
            .AddSingleton<IDatabaseProvider, DatabaseProvider>()
            .AddSingleton<IApplicationLogsRepository, ApplicationLogsRepository>();

        return services;
    }
}