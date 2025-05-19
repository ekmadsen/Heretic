using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Contracts.Internal.Dtos;


public record LogData
{
    public DateTimeOffset Timestamp { get; init; }
    public Guid? CorrelationId { get; set; }
    public string Application { get; init; }
    public string Component { get; init; }
    public string Category { get; init; }
    public LogLevel LogLevel { get; init; }
    public EventId EventId { get; init; }
    public string Message { get; init; }
    public Dictionary<string, object> Properties { get; init; }
}