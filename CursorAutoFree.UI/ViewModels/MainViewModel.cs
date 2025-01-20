using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CursorAutoFree.Core.Services;

namespace CursorAutoFree.UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private readonly ILogService _logService;
    private readonly IBrowserUtils _browserUtils;
    private readonly EmailVerificationHandler _emailHandler;
    private string _emailPrefix;  // 移除默认值
    private string _randomName;   // 移除默认值
    private string _username;
    private double _progress;
    private string _logText;
    private bool _isBusy;

    public event PropertyChangedEventHandler PropertyChanged;

    public MainViewModel(
        IAuthService authService,
        ILogService logService,
        IBrowserUtils browserUtils,
        EmailVerificationHandler emailHandler)
    {
        _authService = authService;
        _logService = logService;
        _browserUtils = browserUtils;
        _emailHandler = emailHandler;
        StartCommand = new Command(OnStart, () => !IsBusy);
        
        // 生成随机用户名
        Username = GenerateRandomUsername();
        
        // 订阅日志更新
        _logService.OnLogAdded += (sender, entry) =>
        {
            LogText += $"[{entry.Timestamp:HH:mm:ss}] {entry.Message}\n";
            if (!string.IsNullOrEmpty(entry.Exception))
            {
                LogText += $"错误: {entry.Exception}\n";
            }
        };
    }

    private string GenerateRandomUsername()
    {
        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var length = random.Next(8, 13); // 生成8-12位的随机用户名
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                ((Command)StartCommand).ChangeCanExecute();
            }
        }
    }

    public string EmailPrefix
    {
        get => _emailPrefix;
        set => SetProperty(ref _emailPrefix, value);
    }

    public string RandomName
    {
        get => _randomName;
        set => SetProperty(ref _randomName, value);
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public double Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    public string LogText
    {
        get => _logText;
        set => SetProperty(ref _logText, value);
    }

    public ICommand StartCommand { get; }

    [Obsolete]
    private async void OnStart()
    {
        if (string.IsNullOrWhiteSpace(EmailPrefix) || 
            string.IsNullOrWhiteSpace(RandomName) || 
            string.IsNullOrWhiteSpace(Username))
        {
            await Application.Current.MainPage.DisplayAlert("提示", "请填写所有必填字段", "确定");
            return;
        }

        Progress = 0;
        LogText = string.Empty;
        IsBusy = true;

        try
        {
            _logService.Log("INFO", "=== 开始注册流程 ===");
            _logService.Log("INFO", $"邮箱域名: {EmailPrefix}");
            _logService.Log("INFO", $"临时邮箱: {RandomName}");
            _logService.Log("INFO", $"注册用户: {Username}");

            Progress = 0.2;
            var registerEmail = $"{Username}@{EmailPrefix}";
            var tempEmail = $"{RandomName}@{EmailPrefix}";

            // 初始化浏览器
            await _browserUtils.InitBrowserAsync();
            await _browserUtils.InstallTurnstilePatchAsync();
            Progress = 0.4;

            // 获取验证码
            var verificationCode = await _browserUtils.GetVerificationCodeAsync(registerEmail);
            if (string.IsNullOrEmpty(verificationCode))
            {
                throw new Exception("无法获取验证码");
            }
            Progress = 0.6;

            // 等待验证码发送到临时邮箱
            _logService.Log("INFO", "等待验证码发送到邮箱...");
            await Task.Delay(5000);

            // 获取验证码
            string code = null;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _logService.Log("INFO", $"第 {i + 1} 次尝试获取验证码");
                    code = await _emailHandler.GetVerificationCodeAsync(tempEmail);
                    if (!string.IsNullOrEmpty(code))
                    {
                        break;
                    }
                    _logService.Log("INFO", $"第 {i + 1} 次尝试获取验证码失败，等待5秒后重试...");
                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    _logService.Log("ERROR", $"第 {i + 1} 次获取验证码时发生错误", ex);
                    if (i == 2)
                    {
                        throw new Exception("获取验证码失败，请重试", ex);
                    }
                    await Task.Delay(5000);
                }
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("无法从邮箱获取验证码");
            }
            Progress = 0.8;

            // 输入验证码完成注册
            await _browserUtils.InputVerificationCodeAsync(code);
            Progress = 1.0;

            _logService.Log("INFO", "=== 注册流程完成 ===");
            await Application.Current.MainPage.DisplayAlert("成功", "注册成功！", "确定");
        }
        catch (Exception ex)
        {
            _logService.Log("ERROR", "注册过程中发生错误", ex);
            await Application.Current.MainPage.DisplayAlert("错误", ex.Message, "确定");
        }
        finally
        {
            IsBusy = false;
            await _browserUtils.DisposeAsync();
        }
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        return false;
    }
} 