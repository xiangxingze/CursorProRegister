using CursorAutoFree.Core.Constants;

namespace CursorAutoFree.Core.Configuration;

public class AppSettings
{
    public AppSettings()
    {
        // 设置默认值
        BrowserUserAgent = AppConstants.Browser.DEFAULT_USER_AGENT;
        RetryAttempts = AppConstants.Config.DEFAULT_RETRY_ATTEMPTS;
        RetryDelay = AppConstants.Config.DEFAULT_RETRY_DELAY;
        LogLevel = AppConstants.Config.DEFAULT_LOG_LEVEL;
        LogPath = AppConstants.Config.DEFAULT_LOG_PATH;
    }

    // 邮箱设置
    public string EmailServer { get; set; }
    public string EmailUsername { get; set; }
    public string EmailPassword { get; set; }
    public int EmailPort { get; set; }
    public bool UseSSL { get; set; }

    // 浏览器设置
    public string BrowserUserAgent { get; set; }
    public bool UseHeadless { get; set; }
    public string BrowserType { get; set; } // Chrome/Firefox/Edge
    public string BrowserPath { get; set; }
    public bool DisableAutomationMode { get; set; }

    // 代理设置
    public bool UseProxy { get; set; }
    public string ProxyServer { get; set; }
    public int ProxyPort { get; set; }
    public string ProxyUsername { get; set; }
    public string ProxyPassword { get; set; }

    // 自动化设置
    public int RetryAttempts { get; set; }
    public int RetryDelay { get; set; } // 秒
    public bool AutoRestart { get; set; }
    public int AutoRestartInterval { get; set; } // 分钟
    public bool SaveLogs { get; set; }
    public string LogPath { get; set; }
    public string LogLevel { get; set; }
} 