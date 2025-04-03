namespace ErikTheCoder.Logging.Settings;


// ReSharper disable PropertyCanBeMadeInitOnly.Global
public record FileLoggerSettings : LoggerSettings
{
    public string Filename { get; set; }
    public bool OverwriteFile { get; set; }
    public FileLoggerColumnSettings Columns { get; set; } = new();
    public bool IncludeProperties { get; set; }
}