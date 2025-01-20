using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using CursorAutoFree.Core.Models;

namespace CursorAutoFree.Core.Services;



public class LogService : ILogService
{
    private readonly ILogger<LogService> _logger;
    public event EventHandler<LogEntry> OnLogAdded;

    public LogService(ILogger<LogService> logger)
    {
        _logger = logger;
    }

    public void LogInfo(string message, [CallerMemberName] string callerName = "")
    {
        _logger.LogInformation("[{CallerName}] {Message}", callerName, message);
        OnLogAdded?.Invoke(this, new LogEntry 
        { 
            Timestamp = DateTime.Now,
            Message = message,
            Level = LogLevel.Information,
            CallerName = callerName
        });
    }

    public void LogError(Exception ex, string message, [CallerMemberName] string callerName = "")
    {
        _logger.LogError(ex, "[{CallerName}] {Message}", callerName, message);
        OnLogAdded?.Invoke(this, new LogEntry 
        { 
            Timestamp = DateTime.Now,
            Message = $"{message} - {ex.Message}",
            Level = LogLevel.Error,
            CallerName = callerName
        });
    }

    public void LogWarning(string message, [CallerMemberName] string callerName = "")
    {
        _logger.LogWarning("[{CallerName}] {Message}", callerName, message);
        OnLogAdded?.Invoke(this, new LogEntry 
        { 
            Timestamp = DateTime.Now,
            Message = message,
            Level = LogLevel.Warning,
            CallerName = callerName
        });
    }
} 