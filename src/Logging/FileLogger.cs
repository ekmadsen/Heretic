using ErikTheCoder.Logging.Settings;
using ErikTheCoder.Utilities.Extensions;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public class FileLogger : ConcurrentLoggerBase
{
    private FileLoggerSettings _settings;
    private FileStream _fileStream;
    private StreamWriter _streamWriter;
    private bool _haveWrittenHeader;
    private bool _disposed;


    public FileLogger(IOptions<FileLoggerSettings> settings) : base(settings)
    {
        _settings = settings.Value;

        var fileMode = _settings.OverwriteFile ? FileMode.Create : FileMode.Append;
        _fileStream = new FileStream(_settings.Filename, fileMode, FileAccess.Write);
        _streamWriter = new StreamWriter(_fileStream) { AutoFlush = false };
    }


    ~FileLogger() => Dispose(false);


    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Free managed resources.
            _settings = null;
        }

        // Free unmanaged resources.
        _streamWriter?.Dispose();
        _streamWriter = null;
        _fileStream?.Dispose();
        _fileStream = null;

        base.Dispose(disposing);
        _disposed = true;
    }


    protected override async Task WriteLogToDataStore(LogData data)
    {
        if (_streamWriter == null) return;

        if (!_haveWrittenHeader)
        {
            // Write file header.
            await _streamWriter.WriteLineAsync($"{"Timestamp".PadRight(_settings.Columns.TimestampWidth)}  {"Application".PadRight(_settings.Columns.ApplicationWidth)}  {"Component".PadRight(_settings.Columns.ComponentWidth)}  {"Class".PadRight(_settings.Columns.ClassWidth)}  {"Level".PadRight(_settings.Columns.LevelWidth)}  {"Event ID".PadRight(_settings.Columns.EventIdWidth)}  Message");
            await _streamWriter.WriteLineAsync($"{new string('=', _settings.Columns.TimestampWidth)}  {new string('=', _settings.Columns.ApplicationWidth)}  {new string('=', _settings.Columns.ComponentWidth)}  {new string('=', _settings.Columns.ClassWidth)}  {new string('=', _settings.Columns.LevelWidth)}  {new string('=', _settings.Columns.EventIdWidth)}  =======");
            _haveWrittenHeader = true;
        }

        // Write log message.
        await _streamWriter.WriteLineAsync($"{data.Timestamp.ToString(_settings.Columns.TimestampFormat)}  {(data.Application ?? "").PadRight(_settings.Columns.ApplicationWidth)}  {(data.Component ?? "").PadRight(_settings.Columns.ComponentWidth)}  {(data.Class ?? "").PadRight(_settings.Columns.ClassWidth)}  {data.LogLevel.ToString().PadRight(_settings.Columns.LevelWidth)}  {data.EventId.ToString().PadRight(_settings.Columns.EventIdWidth)}  {data.Message}");
        if (!_settings.IncludeProperties || data.Properties.IsNullOrEmpty()) return;

        // Write log properties.
        await _streamWriter.WriteAsync(new string(' ', _settings.Columns.TimestampWidth + _settings.Columns.ApplicationWidth + _settings.Columns.ComponentWidth + _settings.Columns.ClassWidth + _settings.Columns.LevelWidth + _settings.Columns.EventIdWidth + 12));
        await _streamWriter.WriteAsync('[');
        
        var index = 0;
        var lastIndex = data.Properties.Count - 1;
        foreach (var (key, value) in data.Properties)
        {
            await _streamWriter.WriteAsync($"\"{key}\": \"{value}\"");
            if (index == lastIndex) break;
            await _streamWriter.WriteAsync(", ");
            index++;
        }

        await _streamWriter.WriteLineAsync(']');
    }


    protected override async Task FlushLogsToDataStore()
    {
        if (_streamWriter == null) return;
        await _streamWriter.FlushAsync();
    }
}