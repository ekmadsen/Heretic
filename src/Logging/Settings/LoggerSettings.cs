using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Logging.Settings;


// ReSharper disable PropertyCanBeMadeInitOnly.Global
public record LoggerSettings
{
    public string Application { get; set; }
    public string Component { get; set; }
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public int QueueIntervalMs { get; set; } = 100;
}