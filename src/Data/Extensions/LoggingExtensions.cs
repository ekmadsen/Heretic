using ErikTheCoder.Heretic.Contracts.Internal;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Data.Extensions;


public static partial class LoggingExtensions
{
    [LoggerMessage(EventId = Event.ExecuteDataReaderId, EventName = Event.ExecuteDataReaderName, Level = LogLevel.Information, Message = "Executing data reader for command text = {commandText}.")]
    public static partial void ExecuteDataReader(this ILogger logger, string commandText);


    [LoggerMessage(EventId = Event.OpenDatabaseId, EventName = Event.OpenDatabaseName, Level = LogLevel.Information, Message = "Opening {server}/{database} database.")]
    public static partial void OpenDatabase(this ILogger logger, string server, string database);
}
