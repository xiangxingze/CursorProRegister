using Microsoft.Playwright;

namespace CursorAutoFree.Core.Services;

public interface IBrowserUtils : IAsyncDisposable
{
    IPage? CurrentPage { get; }
    Task<IPage> InitBrowserAsync();
    Task InstallTurnstilePatchAsync();
    Task<string?> GetVerificationCodeAsync(string email);
    Task InputVerificationCodeAsync(string code);
    Task HandleTurnstileAsync(IPage page);
    Task<string?> GetCursorSessionTokenAsync(IPage page);
} 