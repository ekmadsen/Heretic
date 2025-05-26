using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Logging.Options;
using ErikTheCoder.Utilities.Extensions;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging.Services;


public class FileLogger : ConcurrentLoggerBase
{
    private FileLoggerOptions _options;
    private FileStream _fileStream;
    private StreamWriter _streamWriter;
    private bool _haveWrittenHeader;


    public FileLogger(IOptions<FileLoggerOptions> options) : base(options)
    {
        _options = options.Value;

        var fileMode = _options.OverwriteFile ? FileMode.Create : FileMode.Append;
        _fileStream = new FileStream(_options.Filename, fileMode, FileAccess.Write);
        _streamWriter = new StreamWriter(_fileStream) { AutoFlush = false };
    }


    ~FileLogger() => DisposeInternal();


    protected override void DisposeInternal()
    {
        // Free unmanaged resources.
        _streamWriter?.Dispose();
        _streamWriter = null;
        _fileStream?.Dispose();
        _fileStream = null;

        // Free managed resources.
        _options = null;

        base.DisposeInternal();
    }


    protected override async ValueTask DisposeInternalAsync()
    {
        // Free unmanaged resources.
        if (_streamWriter != null) await _streamWriter.DisposeAsync();
        _streamWriter = null;
        if (_fileStream != null) await _fileStream.DisposeAsync();
        _fileStream = null;

        // Free managed resources.
        _options = null;

        await base.DisposeInternalAsync();
    }
    
    
    protected override async Task WriteLogToDataStore(LogData data)
    {
        if (_streamWriter == null) return;

        if (!_haveWrittenHeader)
        {
            // Write file header.
            await _streamWriter.WriteLineAsync($"{"Timestamp".PadRight(_options.Columns.TimestampWidth)}  {"Correlation ID".PadRight(_options.Columns.CorrelationIdWidth)}  {"Application".PadRight(_options.Columns.ApplicationWidth)}  {"Component".PadRight(_options.Columns.ComponentWidth)}  {"Category".PadRight(_options.Columns.CategoryWidth)}  {"Level".PadRight(_options.Columns.LevelWidth)}  Event ID  {"Event Name".PadRight(_options.Columns.EventNameWidth)}  Message");
            await _streamWriter.WriteLineAsync($"{new string('=', _options.Columns.TimestampWidth)}  {new string('=', _options.Columns.CorrelationIdWidth)}  {new string('=', _options.Columns.ApplicationWidth)}  {new string('=', _options.Columns.ComponentWidth)}  {new string('=', _options.Columns.CategoryWidth)}  {new string('=', _options.Columns.LevelWidth)}  ========  {new string('=', _options.Columns.EventNameWidth)}  =======");
            _haveWrittenHeader = true;
        }

        // Write log message.
        await _streamWriter.WriteLineAsync($"{data.Timestamp.ToString(_options.Columns.TimestampFormat)}  {(data.CorrelationId?.ToString() ?? "").PadRight(_options.Columns.CorrelationIdWidth)}  {(data.Application ?? "").PadRight(_options.Columns.ApplicationWidth)}  {(data.Component ?? "").PadRight(_options.Columns.ComponentWidth)}  {(data.Category ?? "").PadRight(_options.Columns.CategoryWidth)}  {data.LogLevel.ToString().PadRight(_options.Columns.LevelWidth)}  {data.EventId.Id:00000000}  {(data.EventId.Name ?? "").PadRight(_options.Columns.EventNameWidth)}  {data.Message}");
        if (!_options.IncludeProperties || data.Properties.IsNullOrEmpty()) return;

        // Write log properties.
        await _streamWriter.WriteAsync(new string(' ', _options.Columns.TimestampWidth + _options.Columns.CorrelationIdWidth + _options.Columns.ApplicationWidth + _options.Columns.ComponentWidth + _options.Columns.CategoryWidth + _options.Columns.LevelWidth + _options.Columns.EventNameWidth + 24));
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