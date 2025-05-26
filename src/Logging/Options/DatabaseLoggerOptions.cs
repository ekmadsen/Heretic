namespace ErikTheCoder.Logging.Options;


public record DatabaseLoggerOptions : LoggerOptions
{
    public string Connection { get; set; }
}