using CursorAutoFree.Core.Configuration;

namespace CursorAutoFree.Core.Services;

public interface ISettingsService
{
    AppSettings GetSettings();
    Task SaveSettingsAsync(AppSettings settings);
} 