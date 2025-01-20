using CursorAutoFree.Core.Models;

namespace CursorAutoFree.Core.Services;

public interface ILogService
{
    IReadOnlyList<LogEntry> Logs { get; }
    void Log(string level, string message, Exception? exception = null);
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception = null);
    void Clear();
    event EventHandler<LogEntry>? OnLogAdded;
} 