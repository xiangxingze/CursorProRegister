using CursorAutoFree.Core.Models;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace CursorAutoFree.Core.Services;

public interface ILogService
{
    void LogInfo(string message, [CallerMemberName] string callerName = "");
    void LogError(Exception ex, string message, [CallerMemberName] string callerName = "");
    void LogWarning(string message, [CallerMemberName] string callerName = "");

    event EventHandler<LogEntry> OnLogAdded;
}