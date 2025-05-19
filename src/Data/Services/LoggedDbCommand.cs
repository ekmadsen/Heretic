using System.Data;
using System.Data.Common;
using ErikTheCoder.Data.Extensions;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Data.Services;

public class LoggedDbCommand(ILogger<DbCommand> logger, DbCommand command) : DbCommand
{
    protected override DbConnection DbConnection
    {
        get => command.Connection;
        set => command.Connection = value;
    }


    protected override DbTransaction DbTransaction
    {
        get => command.Transaction;
        set => command.Transaction = value;
    }


    public override CommandType CommandType
    {
        get => command.CommandType;
        set => command.CommandType = value;
    }


    public override UpdateRowSource UpdatedRowSource
    {
        get => command.UpdatedRowSource;
        set => command.UpdatedRowSource = value;
    }


    protected override DbParameterCollection DbParameterCollection => command.Parameters;


    public override string CommandText
    {
        get => command.CommandText;
        set => command.CommandText = value;
    }


    public override int CommandTimeout
    {
        get => command.CommandTimeout;
        set => command.CommandTimeout = value;
    }


    public override bool DesignTimeVisible
    {
        get => command.DesignTimeVisible;
        set => command.DesignTimeVisible = value;
    }


    protected override DbParameter CreateDbParameter() => command.CreateParameter();


    public override void Prepare()
    {
        // TODO: Log preparing command.
        command.Prepare();
    }


    public override int ExecuteNonQuery()
    {
        // TODO: Log executing a non query statement.
        return command.ExecuteNonQuery();
    }


    public override object ExecuteScalar()
    {
        // TODO: Log executing a scalar statement.
        return command.ExecuteScalar();
    }


    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        logger.ExecuteDataReader(command.CommandText);
        return command.ExecuteReader();
    }


    public override void Cancel() => command.Cancel();
}