using Microsoft.Extensions.Logging;

namespace CursorAutoFree.Core.Models;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public LogLevel Level { get; set; }
    public string CallerName { get; set; }
} 