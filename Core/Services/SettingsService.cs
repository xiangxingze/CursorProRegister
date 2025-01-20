using System.Text.Json;
using CursorAutoFree.Core.Configuration;
using Microsoft.Extensions.Logging;

namespace CursorAutoFree.Core.Services;

public class SettingsService : ISettingsService
{
    private readonly string _settingsPath;
    private readonly ILogger<SettingsService> _logger;
    private AppSettings _settings;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
        _settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CursorAutoFree",
            "settings.json"
        );
        LoadSettings();
    }

    public AppSettings GetSettings() => _settings;

    public async Task SaveSettingsAsync(AppSettings settings)
    {
        try
        {
            _settings = settings;
            var directory = Path.GetDirectoryName(_settingsPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(_settingsPath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save settings");
            throw;
        }
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                _settings = JsonSerializer.Deserialize<AppSettings>(json);
            }
            else
            {
                _settings = new AppSettings();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load settings");
            _settings = new AppSettings();
        }
    }
} 