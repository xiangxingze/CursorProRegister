using CursorAutoFree.Core.Models;
using CursorAutoFree.Core.Services;

namespace CursorAutoFree.Infrastructure.Logging;

public class LogService : ILogService
{
    private readonly List<LogEntry> _logs = new();
    public IReadOnlyList<LogEntry> Logs => _logs.AsReadOnly();
    public event EventHandler<LogEntry>? OnLogAdded;

    public void Log(string level, string message, Exception? exception = null)
    {
        var entry = new LogEntry(DateTime.Now, level, message, exception?.ToString());
        _logs.Add(entry);
        OnLogAdded?.Invoke(this, entry);
    }

    public void LogInfo(string message)
    {
        Log("INFO", message);
    }

    public void LogWarning(string message)
    {
        Log("WARNING", message);
    }

    public void LogError(string message, Exception? exception = null)
    {
        Log("ERROR", message, exception);
    }

    public void Clear()
    {
        _logs.Clear();
    }
} 