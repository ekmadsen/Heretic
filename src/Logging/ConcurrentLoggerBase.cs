using System.Collections.Concurrent;
using ErikTheCoder.Logging.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public abstract class ConcurrentLoggerBase : ILogger, IDisposable
{
    private LoggerSettings _settings;
    private ConcurrentQueue<LogData> _queue; // Thread-safe.
    private Timer _timer;
    private bool _disposed;


    protected ConcurrentLoggerBase(IOptions<LoggerSettings> settings)
    {
        _settings = settings.Value;

        // Create queue to hold log data.
        // This enables zero-latency logging by deferring I/O cost of writing logs to data store.
        _queue = new ConcurrentQueue<LogData>();

        // Create timer that drains queue on an interval, writing logs to data store.
        // Timer calls method on a ThreadPool thread.
        _timer = new Timer(WriteLogs, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(settings.Value.QueueIntervalMs));
    }


    ~ConcurrentLoggerBase() => Dispose(false);


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        // Wait for queue to drain.
        while (!_queue.IsEmpty)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(_settings.QueueIntervalMs));
        }

        if (disposing)
        {
            // Free managed resources.
            _settings = null;
            _queue = null;
        }

        // Free unmanaged resources.
        _timer?.Dispose();
        _timer = null;

        _disposed = true;
    }


    public IDisposable BeginScope<TState>(TState state) where TState : notnull => null; // Not supported.


    public bool IsEnabled(LogLevel logLevel) => logLevel >= _settings.LogLevel;


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
        Log(logLevel, eventId, state, exception, formatter, null);


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, string category)
    {
        if (!IsEnabled(logLevel)) return;

        // Add log data to queue.
        var data = GetLogData(logLevel, eventId, state, exception, formatter, category);
        _queue.Enqueue(data);
    }


    private LogData GetLogData<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, string category) => new()
    {
        Timestamp = DateTimeOffset.Now,
        Application = _settings.Application,
        Component = _settings.Component,
        Class = category,
        LogLevel = logLevel,
        EventId = eventId,
        Message = formatter(state, exception),
        Properties = state is IEnumerable<KeyValuePair<string, object>> properties ? properties.ToDictionary() : []
    };


    // Usually async void is an anti-pattern.
    // However, it's appropriate for event handlers.  See https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming.
    // ReSharper disable once AsyncVoidMethod
    private async void WriteLogs(object state)
    {
        try
        {
            // Prevent overlap if method execution time exceeds timer interval.
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);

            // Drain queue, writing logs to data store.
            while (_queue?.TryDequeue(out var data) ?? false)
            {
                if (data == null) continue;
                await WriteLogToDataStore(data);
            }
        }
        finally
        {
            await FlushLogsToDataStore();

            // Enable timer to fire again.
            _timer?.Change(_settings.QueueIntervalMs, _settings.QueueIntervalMs);
        }
    }


    protected abstract Task WriteLogToDataStore(LogData data);
    protected abstract Task FlushLogsToDataStore();
}