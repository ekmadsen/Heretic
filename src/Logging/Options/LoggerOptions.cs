using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Logging.Options;


public record LoggerOptions
{
    public string Application { get; set; }
    public string Component { get; set; }
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public string CorrelationIdPropertyName { get; set; }
    public int QueueIntervalMs { get; set; } = 100;
}