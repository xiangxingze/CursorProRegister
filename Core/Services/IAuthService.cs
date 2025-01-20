using CursorAutoFree.Core.Models;

namespace CursorAutoFree.Core.Services;

public interface IAuthService
{
    Task<AuthInfo> GetAuthInfoAsync();
    Task<bool> VerifyLicenseAsync(string licenseKey);
    Task<bool> DecrementAttemptsAsync();
    Task<bool> IsAuthorizedAsync();
    Task<int> GetRemainingAttemptsAsync();
    Task ResetAttemptsAsync();
} 