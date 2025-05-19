namespace ErikTheCoder.Logging.Options;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
public record FileLoggerColumnOptions
{
    public string TimestampFormat { get; set; } = "O";
    public int TimestampWidth { get; set; } = 33;
    public int CorrelationIdWidth { get; set; } = 36;
    public int ApplicationWidth { get; set; } = 20;
    public int ComponentWidth { get; set; } = 20;
    public int LevelWidth { get; set; } = 11;
    public int CategoryWidth { get; set; } = 100;
    public int EventNameWidth { get; set; } = 50;
}
