using ErikTheCoder.Heretic.Contracts.Internal;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Data.Extensions;


public static partial class LoggingExtensions
{
    [LoggerMessage(EventId = Event.OpenDatabaseId, EventName = Event.OpenDatabaseName, Level = LogLevel.Information, Message = "Opening {server}/{database} database.")]
    public static partial void OpenDatabase(this ILogger logger, string server, string database);


    [LoggerMessage(EventId = Event.ExecuteDataReaderId, EventName = Event.ExecuteDataReaderName, Level = LogLevel.Information, Message = "Executing data reader for command text = {commandText}.")]
    public static partial void ExecuteDataReader(this ILogger logger, string commandText);


    [LoggerMessage(EventId = Event.CommitTransactionId, EventName = Event.CommitTransactionName, Level = LogLevel.Information, Message = "Committing transaction.")]
    public static partial void CommitTransaction(this ILogger logger);


    [LoggerMessage(EventId = Event.RollbackTransactionId, EventName = Event.RollbackTransactionName, Level = LogLevel.Information, Message = "Rolling back transaction.")]
    public static partial void RollbackTransaction(this ILogger logger);
}
