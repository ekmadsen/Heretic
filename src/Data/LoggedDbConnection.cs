using System.Data;
using System.Data.Common;
using ErikTheCoder.Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Data;


public class LoggedDbConnection(ILogger<DbConnection> logger, IServiceProvider serviceProvider, DbConnection connection) : DbConnection
{
    public override string ConnectionString
    {
        get => connection.ConnectionString;
        set => connection.ConnectionString = value;
    }


    public override string DataSource => connection.DataSource;
    public override string ServerVersion => connection.ServerVersion;
    public override string Database => connection.Database;
    public override ConnectionState State => connection.State;


    public override void ChangeDatabase(string databaseName)
    {
        // TODO: Log changing database.
        connection.ChangeDatabase(databaseName);
    }
    
    
    public override void Open()
    {
        logger.OpenDatabase(connection.DataSource ?? string.Empty, connection.Database);
        connection.Open();
    }


    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => new LoggedDbTransaction(serviceProvider.GetService<ILogger<LoggedDbTransaction>>(), connection.BeginTransaction());
    protected override DbCommand CreateDbCommand() => new LoggedDbCommand(serviceProvider.GetService<ILogger<LoggedDbCommand>>(), connection.CreateCommand());


    public override void Close()
    {
        // TODO: Log closing database.
        connection.Close();
    }
}