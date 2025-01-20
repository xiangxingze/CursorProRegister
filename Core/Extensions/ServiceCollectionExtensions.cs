using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CursorAutoFree.Core.Services;
using CursorAutoFree.Core.Configuration;
using CursorAutoFree.Core.Constants;

namespace CursorAutoFree.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCursorAutoFreeCore(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IAuthService, AuthService>();
        
        // 添加配置服务
        services.AddOptions<AppSettings>().Configure(settings =>
        {
            settings.BrowserUserAgent = AppConstants.Browser.DEFAULT_USER_AGENT;
            settings.RetryAttempts = AppConstants.Config.DEFAULT_RETRY_ATTEMPTS;
            settings.RetryDelay = AppConstants.Config.DEFAULT_RETRY_DELAY;
            settings.LogLevel = AppConstants.Config.DEFAULT_LOG_LEVEL;
            settings.LogPath = AppConstants.Config.DEFAULT_LOG_PATH;
        });

        return services;
    }
} 