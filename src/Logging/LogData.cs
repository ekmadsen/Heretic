using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Logging;


public record LogData
{
    public DateTimeOffset Timestamp { get; init; }
    public string Application { get; init; }
    public string Component { get; init; }
    public string Class { get; init; }
    public LogLevel LogLevel { get; init; }
    public EventId EventId { get; init; }
    public string Message { get; init; }
    public Dictionary<string, object> Properties { get; init; }
}