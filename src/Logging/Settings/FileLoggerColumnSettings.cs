namespace ErikTheCoder.Logging.Settings;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
public record FileLoggerColumnSettings
{
    public string TimestampFormat { get; set; } = "O";
    public int TimestampWidth { get; set; } = 33;
    public int ApplicationWidth { get; set; } = 20;
    public int ComponentWidth { get; set; } = 20;
    public int LevelWidth { get; set; } = 11;
    public int ClassWidth { get; set; } = 100;
    public int EventIdWidth { get; set; } = 40;
}
