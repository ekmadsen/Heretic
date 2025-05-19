namespace ErikTheCoder.Logging.Options;


// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
public record FileLoggerOptions : LoggerOptions
{
    public string Filename { get; set; }
    public bool OverwriteFile { get; set; }
    public FileLoggerColumnOptions Columns { get; set; } = new();
    public bool IncludeProperties { get; set; }
}