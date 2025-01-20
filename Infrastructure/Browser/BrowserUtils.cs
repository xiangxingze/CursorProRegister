using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using CursorAutoFree.Core.Services;

namespace CursorAutoFree.Infrastructure.Browser;

/// <summary>
/// 浏览器工具类，实现浏览器自动化的核心功能
/// </summary>
public class BrowserUtils : IBrowserUtils
{
    private readonly ILogger<BrowserUtils> _logger;
    private readonly IConfiguration _configuration;
    private IBrowser? _browser;
    private IPlaywright? _playwright;
    private IPage? _page;
    private bool _disposed;
    private const string SIGN_UP_URL = "https://authenticator.cursor.sh/sign-up";

    public IPage? CurrentPage => _page;

    public BrowserUtils(ILogger<BrowserUtils> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<IPage> InitBrowserAsync()
    {
        try
        {
            _logger.LogInformation("\n=== 初始化程序 ===");
            
            // 先退出已有的 Chrome 进程
            await CleanupExistingBrowsers();
            
            _logger.LogInformation("正在初始化浏览器...");
            _playwright = await Playwright.CreateAsync();

            // 配置浏览器选项
            var options = new BrowserTypeLaunchOptions
            {
                Channel = "chrome",  // 使用本地Chrome
                Headless = true,    // 显示浏览器窗口
                Args = new[]
                {
                    "--hide-crash-restore-bubble",
                    "--disable-blink-features=AutomationControlled",
                    "--no-sandbox",
                    "--disable-gpu"
                }
            };

            // 从配置或环境变量获取代理设置
            var proxy = _configuration["Browser:Proxy"];
            if (!string.IsNullOrEmpty(proxy))
            {
                options.Proxy = new Proxy { Server = proxy };
            }

            _browser = await _playwright.Chromium.LaunchAsync(options);
            
            // 创建新的上下文，设置 UserAgent
            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.6723.92 Safari/537.36"
            });

            _page = await context.NewPageAsync();

            // 读取并注入补丁脚本
            var rootDir = AppDomain.CurrentDomain.BaseDirectory;
            var scriptPath = Path.Combine(rootDir, "turnstilePatch", "script.js");

            if (!File.Exists(scriptPath))
            {
                var infrastructurePath = Path.Combine(rootDir, "..", "..", "..", "Infrastructure", "turnstilePatch", "script.js");
                scriptPath = Path.GetFullPath(infrastructurePath);
            }

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException("找不到补丁脚本文件");
            }

            var script = await File.ReadAllTextAsync(scriptPath);
            await _page.AddInitScriptAsync(script);

            _logger.LogInformation("浏览器初始化完成");
            return _page;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "浏览器初始化失败");
            throw;
        }
    }

    private async Task CleanupExistingBrowsers()
    {
        try
        {
            _logger.LogInformation("清理已有的浏览器进程...");
            
            if (_page != null)
            {
                await _page.CloseAsync();
                _page = null;
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }

            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }

            // 在 Windows 上尝试结束 Chrome 进程
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    var process = new System.Diagnostics.Process
                    {
                        StartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "taskkill",
                            Arguments = "/F /IM chrome.exe",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    await process.WaitForExitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "清理Chrome进程失败");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "清理现有浏览器时发生错误");
        }
    }

    public async Task InstallTurnstilePatchAsync()
    {
        if (_page == null)
        {
            throw new InvalidOperationException("Browser not initialized");
        }

        try
        {
            _logger.LogInformation("Installing Turnstile patch...");
            var rootDir = AppDomain.CurrentDomain.BaseDirectory;
            var extensionPath = Path.Combine(rootDir, "turnstilePatch");

            if (!Directory.Exists(extensionPath))
            {
                var infrastructurePath = Path.Combine(rootDir, "..", "..", "..", "Infrastructure", "turnstilePatch");
                extensionPath = Path.GetFullPath(infrastructurePath);
            }

            if (!Directory.Exists(extensionPath))
            {
                _logger.LogError("补丁扩展目录不存在");
                throw new DirectoryNotFoundException("补丁扩展目录不存在");
            }

            var manifestPath = Path.Combine(extensionPath, "manifest.json");
            var scriptPath = Path.Combine(extensionPath, "script.js");

            if (!File.Exists(manifestPath) || !File.Exists(scriptPath))
            {
                _logger.LogError("补丁文件不完整");
                throw new FileNotFoundException("补丁文件不完整");
            }

            _logger.LogInformation("Turnstile patch files verified");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify Turnstile patch");
            throw;
        }
    }

    public async Task<string?> GetVerificationCodeAsync(string email)
    {
        if (_page == null)
        {
            throw new InvalidOperationException("Browser not initialized");
        }

        try
        {
            _logger.LogInformation("=== 开始注册账号流程 ===");
            _logger.LogInformation($"正在访问注册页面: {SIGN_UP_URL}");

            // 打开注册页面
            await _page.GotoAsync(SIGN_UP_URL);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // 检查页面是否包含注册表单
            var firstNameInput = await _page.QuerySelectorAsync("input[name='first_name']");
            if (firstNameInput != null)
            {
                _logger.LogInformation("正在填写个人信息...");

                // 生成随机姓名
                var firstName = GenerateRandomName(5, 8);
                var lastName = GenerateRandomName(5, 8);

                // 填写名字
                await firstNameInput.ClickAsync();
                await firstNameInput.FillAsync(firstName);
                _logger.LogInformation($"已输入名字: {firstName}");
                await Task.Delay(Random.Shared.Next(1000, 3000));

                // 填写姓氏
                var lastNameInput = await _page.QuerySelectorAsync("input[name='last_name']");
                if (lastNameInput != null)
                {
                    await lastNameInput.ClickAsync();
                    await lastNameInput.FillAsync(lastName);
                    _logger.LogInformation($"已输入姓氏: {lastName}");
                    await Task.Delay(Random.Shared.Next(1000, 3000));
                }
                else
                {
                    throw new Exception("找不到姓氏输入框");
                }

                // 填写邮箱
                var emailInput = await _page.QuerySelectorAsync("input[name='email']");
                if (emailInput != null)
                {
                    await emailInput.ClickAsync();
                    await emailInput.FillAsync(email);
                    _logger.LogInformation($"已输入邮箱: {email}");
                    await Task.Delay(Random.Shared.Next(1000, 3000));
                }
                else
                {
                    throw new Exception("找不到邮箱输入框");
                }

                // 提交表单
                _logger.LogInformation("提交个人信息...");
                var submitButton = await _page.QuerySelectorAsync("button[type='submit']");
                if (submitButton != null)
                {
                    await submitButton.ClickAsync();
                }
                else
                {
                    throw new Exception("找不到提交按钮");
                }

                // 处理 Turnstile 验证
                await HandleTurnstileAsync(_page);
                _logger.LogInformation("已完成 Turnstile 验证");

                // 等待成功提示或错误消息
                try
                {
                    // 等待成功提示
                    await _page.WaitForSelectorAsync("text=Verification code sent", new PageWaitForSelectorOptions 
                    { 
                        Timeout = 10000 // 10秒超时
                    });
                    _logger.LogInformation("验证码发送成功");
                    return "success";
                }
                catch (TimeoutException)
                {
                    // 检查是否有错误消息
                    var errorText = await _page.TextContentAsync(".error-message") ?? string.Empty;
                    if (!string.IsNullOrEmpty(errorText))
                    {
                        _logger.LogError($"获取验证码失败: {errorText}");
                        throw new Exception(errorText);
                    }
                    _logger.LogError("获取验证码超时");
                    throw new TimeoutException("获取验证码超时，请重试");
                }
            }
            else
            {
                throw new Exception("页面上找不到注册表单");
            }
        }
        catch (Exception ex) when (ex is not TimeoutException)
        {
            _logger.LogError(ex, "注册页面访问失败");
            throw;
        }
    }

    private string GenerateRandomName(int minLength, int maxLength)
    {
        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        var length = random.Next(minLength, maxLength + 1);
        var name = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return char.ToUpper(name[0]) + name.Substring(1); // 首字母大写
    }

    public async Task InputVerificationCodeAsync(string code)
    {
        if (_page == null)
        {
            throw new InvalidOperationException("Browser not initialized");
        }

        // 输入验证码
        await _page.FillAsync("input[type='text']", code);
        
        // 点击验证按钮
        await _page.ClickAsync("button:has-text('Verify')");
        
        // 等待请求完成
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task Cleanup()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
            _page = null;
        }

        if (_browser != null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }

        _playwright?.Dispose();
        _playwright = null;
    }

    public async Task HandleTurnstileAsync(IPage page)
    {
        _logger.LogInformation("正在处理 Turnstile 验证...");
        
        try
        {
            // 等待验证完成
            await page.WaitForFunctionAsync(@"
                () => new Promise((resolve) => {
                    const checkToken = () => {
                        const token = document.querySelector('input[name=""cf-turnstile-response""]')?.value;
                        if (token) {
                            resolve(true);
                            return;
                        }
                        setTimeout(checkToken, 500);
                    };
                    checkToken();
                })
            ", new PageWaitForFunctionOptions { Timeout = 30000 });

            _logger.LogInformation("Turnstile 验证通过");
            await Task.Delay(1000);  // 等待验证结果处理完成
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Turnstile 验证失败");
            throw;
        }
    }

    public async Task<string?> GetCursorSessionTokenAsync(IPage page)
    {
        try
        {
            // 从 localStorage 获取 token
            var token = await page.EvaluateAsync<string>("() => localStorage.getItem('cursor-session')");
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cursor session token");
            return null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_page != null) await _page.CloseAsync();
            if (_browser != null) await _browser.DisposeAsync();
            if (_playwright != null) _playwright.Dispose();

            _page = null;
            _browser = null;
            _playwright = null;
            _disposed = true;
        }
    }
} 