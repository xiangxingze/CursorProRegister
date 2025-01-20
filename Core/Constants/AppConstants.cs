namespace CursorAutoFree.Core.Constants;

public static class AppConstants
{
    public const string DOMAIN = "xiaoye6688.us.kg";
    public const string TEMP_MAIL = "xiaoye6688";
    
    public static class Auth
    {
        /// <summary>
        /// 免费用户最大尝试次数
        /// </summary>
        public const int FREE_USER_MAX_ATTEMPTS = 3;
        /// <summary>
        /// 授权信息文件路径
        /// </summary>
        public const string AUTH_FILE_PATH = "auth.json";
        public const string LICENSE_CHECK_URL = $"https://{DOMAIN}/api/license/verify";
    }
    
    public static class Urls
    {
        public const string BASE_URL = $"https://{DOMAIN}";
        public const string TEMP_MAIL_URL = $"https://{TEMP_MAIL}.com";
    }
    
    public static class Browser
    {
        /// <summary>
        /// 默认用户代理
        /// </summary>
        public const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
        /// <summary>
        /// Turnstile 补丁路径
        /// </summary>
        public const string TURNSTILE_PATCH_PATH = "turnstilePatch";
    }
    
    public static class Config
    {
        public const int DEFAULT_RETRY_ATTEMPTS = 3;
        public const int DEFAULT_RETRY_DELAY = 5; // seconds
        public const string DEFAULT_LOG_LEVEL = "Information";
        public const string DEFAULT_LOG_PATH = "logs/cursor_auto_free.log";
    }
} 