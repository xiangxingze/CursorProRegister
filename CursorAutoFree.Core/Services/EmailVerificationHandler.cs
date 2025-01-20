using CursorAutoFree.Core.Models;
using CursorAutoFree.Core.Services;

namespace CursorAutoFree.Core.Services;

public class EmailVerificationHandler
{
    private readonly ILogService _logService;
    private readonly IBrowserUtils _browserUtils;

    public EmailVerificationHandler(ILogService logService, IBrowserUtils browserUtils)
    {
        _logService = logService;
        _browserUtils = browserUtils;
    }

    public async Task<string?> GetVerificationCodeAsync(string email)
    {
        try
        {
            _logService.Log("INFO", $"正在获取验证码: {email}");
            return await _browserUtils.GetVerificationCodeAsync(email);
        }
        catch (Exception ex)
        {
            _logService.Log("ERROR", $"获取验证码失败: {email}", ex);
            return null;
        }
    }
} 