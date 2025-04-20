using ErikTheCoder.Logging.Options;
using ErikTheCoder.Utilities.Extensions;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public class FileLogger : ConcurrentLoggerBase
{
    private FileLoggerOptions _options;
    private FileStream _fileStream;
    private StreamWriter _streamWriter;
    private bool _haveWrittenHeader;
    private bool _disposed;


    public FileLogger(IOptions<FileLoggerOptions> options) : base(options)
    {
        _options = options.Value;

        var fileMode = _options.OverwriteFile ? FileMode.Create : FileMode.Append;
        _fileStream = new FileStream(_options.Filename, fileMode, FileAccess.Write);
        _streamWriter = new StreamWriter(_fileStream) { AutoFlush = false };
    }


    ~FileLogger() => Dispose(false);


    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Free managed resources.
            _options = null;
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
            await _streamWriter.WriteLineAsync($"{"Timestamp".PadRight(_options.Columns.TimestampWidth)}  {"Correlation ID".PadRight(_options.Columns.CorrelationIdWidth)}  {"Application".PadRight(_options.Columns.ApplicationWidth)}  {"Component".PadRight(_options.Columns.ComponentWidth)}  {"Class".PadRight(_options.Columns.ClassWidth)}  {"Level".PadRight(_options.Columns.LevelWidth)}  Event ID  {"Event Name".PadRight(_options.Columns.EventNameWidth)}  Message");
            await _streamWriter.WriteLineAsync($"{new string('=', _options.Columns.TimestampWidth)}  {new string('=', _options.Columns.CorrelationIdWidth)}  {new string('=', _options.Columns.ApplicationWidth)}  {new string('=', _options.Columns.ComponentWidth)}  {new string('=', _options.Columns.ClassWidth)}  {new string('=', _options.Columns.LevelWidth)}  ========  {new string('=', _options.Columns.EventNameWidth)}  =======");
            _haveWrittenHeader = true;
        }

        // Write log message.
        await _streamWriter.WriteLineAsync($"{data.Timestamp.ToString(_options.Columns.TimestampFormat)}  {data.CorrelationId}  {(data.Application ?? "").PadRight(_options.Columns.ApplicationWidth)}  {(data.Component ?? "").PadRight(_options.Columns.ComponentWidth)}  {(data.Class ?? "").PadRight(_options.Columns.ClassWidth)}  {data.LogLevel.ToString().PadRight(_options.Columns.LevelWidth)}  {data.EventId.Id:00000000}  {(data.EventId.Name ?? "").PadRight(_options.Columns.EventNameWidth)}  {data.Message}");
        if (!_options.IncludeProperties || data.Properties.IsNullOrEmpty()) return;

        // Write log properties.
        await _streamWriter.WriteAsync(new string(' ', _options.Columns.TimestampWidth + _options.Columns.CorrelationIdWidth + _options.Columns.ApplicationWidth + _options.Columns.ComponentWidth + _options.Columns.ClassWidth + _options.Columns.LevelWidth + _options.Columns.EventNameWidth + 24));
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