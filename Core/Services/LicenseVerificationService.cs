using System.Net.Http;
using System.Text.Json;
using CursorAutoFree.Core.Constants;
using CursorAutoFree.Core.Models;
using Microsoft.Extensions.Logging;

namespace CursorAutoFree.Core.Services;

public interface ILicenseVerificationService
{
    Task<LicenseVerificationResult> VerifyLicenseAsync(string licenseKey);
    Task<string> GetPurchaseUrlAsync();
}

public class LicenseVerificationService : ILicenseVerificationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LicenseVerificationService> _logger;

    public LicenseVerificationService(ILogger<LicenseVerificationService> logger)
    {
        _httpClient = new HttpClient();
        _logger = logger;
    }

    public async Task<LicenseVerificationResult> VerifyLicenseAsync(string licenseKey)
    {
        try
        {
            var response = await _httpClient.PostAsync(
                AppConstants.Auth.LICENSE_CHECK_URL,
                new StringContent(JsonSerializer.Serialize(new { licenseKey }))
            );

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LicenseVerificationResult>(content);
                return result;
            }

            return new LicenseVerificationResult 
            { 
                IsValid = false, 
                Message = "验证服务器返回错误" 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "License verification failed");
            return new LicenseVerificationResult 
            { 
                IsValid = false, 
                Message = "验证服务连接失败" 
            };
        }
    }

    public async Task<string> GetPurchaseUrlAsync()
    {
        return $"{AppConstants.Urls.BASE_URL}/purchase";
    }
} 