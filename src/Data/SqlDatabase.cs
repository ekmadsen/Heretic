using System.Data.Common;
using ErikTheCoder.Data.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Data;


public class SqlDatabase(IServiceProvider serviceProvider, IOptions<DatabaseOptions> options) : IDatabase
{
    private readonly DatabaseOptions _options = options.Value;


    public string Name => _options.Name;


    public DbConnection OpenConnection()
    {
        var connection = GetConnection();
        connection.Open();
        return connection;
    }


    public async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = GetConnection();
        await connection.OpenAsync(cancellationToken);
        return connection;
    }


    private DbConnection GetConnection()
    {
        var sqlConnection = new SqlConnection(_options.Connection);

        return _options.LogQueries
            ? new LoggedDbConnection(serviceProvider.GetService<ILogger<DbConnection>>(), serviceProvider, sqlConnection)
            : sqlConnection;
    }
}