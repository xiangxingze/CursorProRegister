namespace CursorAutoFree.Core.Models;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }

    public LogEntry(DateTime timestamp, string level, string message, string? exception = null)
    {
        Timestamp = timestamp;
        Level = level;
        Message = message;
        Exception = exception;
    }
} 