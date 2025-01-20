using System.Text.Json;
using CursorAutoFree.Core.Constants;
using CursorAutoFree.Core.Models;
using Microsoft.Extensions.Logging;

namespace CursorAutoFree.Core.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly string _authFilePath;
    private AuthInfo _authInfo;

    public AuthService(ILogger<AuthService> logger)
    {
        _logger = logger;
        _authFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CursorAutoFree",
            AppConstants.Auth.AUTH_FILE_PATH
        );
        LoadAuthInfo();
    }

    public async Task<AuthInfo> GetAuthInfoAsync()
    {
        return await Task.FromResult(_authInfo);
    }

    public async Task<bool> VerifyLicenseAsync(string licenseKey)
    {
        try
        {
            _authInfo.LicenseKey = licenseKey;
            _authInfo.IsAuthorized = true;
            _authInfo.RemainingAttempts = AppConstants.Auth.FREE_USER_MAX_ATTEMPTS;
            _authInfo.LastUsed = DateTime.Now;
            _authInfo.ExpiryDate = DateTime.Now.AddDays(30);

            await SaveAuthInfoAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify license");
            return false;
        }
    }

    public async Task<bool> DecrementAttemptsAsync()
    {
        if (_authInfo.IsAuthorized)
        {
            return true;
        }

        _authInfo.RemainingAttempts = Math.Max(0, _authInfo.RemainingAttempts - 1);
        _authInfo.LastUsed = DateTime.Now;
        await SaveAuthInfoAsync();

        return _authInfo.RemainingAttempts > 0;
    }

    public async Task<bool> IsAuthorizedAsync()
    {
        if (_authInfo.IsAuthorized)
        {
            if (_authInfo.ExpiryDate.HasValue && _authInfo.ExpiryDate.Value < DateTime.Now)
            {
                _authInfo.IsAuthorized = false;
                await SaveAuthInfoAsync();
                return false;
            }
            return true;
        }

        return _authInfo.RemainingAttempts > 0;
    }

    public async Task<int> GetRemainingAttemptsAsync()
    {
        return await Task.FromResult(_authInfo.RemainingAttempts);
    }

    public async Task ResetAttemptsAsync()
    {
        _authInfo.RemainingAttempts = AppConstants.Auth.FREE_USER_MAX_ATTEMPTS;
        await SaveAuthInfoAsync();
    }

    private void LoadAuthInfo()
    {
        try
        {
            var directory = Path.GetDirectoryName(_authFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            if (File.Exists(_authFilePath))
            {
                var json = File.ReadAllText(_authFilePath);
                _authInfo = JsonSerializer.Deserialize<AuthInfo>(json) ?? new AuthInfo
                {
                    RemainingAttempts = AppConstants.Auth.FREE_USER_MAX_ATTEMPTS
                };
            }
            else
            {
                _authInfo = new AuthInfo
                {
                    RemainingAttempts = AppConstants.Auth.FREE_USER_MAX_ATTEMPTS
                };
                SaveAuthInfoAsync().Wait();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load auth info");
            _authInfo = new AuthInfo
            {
                RemainingAttempts = AppConstants.Auth.FREE_USER_MAX_ATTEMPTS
            };
        }
    }

    private async Task SaveAuthInfoAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_authInfo);
            await File.WriteAllTextAsync(_authFilePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save auth info");
        }
    }
} 