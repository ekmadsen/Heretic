namespace ErikTheCoder.Logging.Options;


// ReSharper disable PropertyCanBeMadeInitOnly.Global
public record DatabaseLoggerOptions : LoggerOptions
{
    public string Connection { get; set; }
}