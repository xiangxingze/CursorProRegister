using Microsoft.Extensions.Logging;
using CursorAutoFree.UI.ViewModels;
using CursorAutoFree.UI.Views;
using CursorAutoFree.Core.Services;
using CursorAutoFree.Infrastructure.Auth;
using CursorAutoFree.Infrastructure.Logging;
using CursorAutoFree.Infrastructure.Browser;

namespace CursorAutoFree.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 注册核心服务
        builder.Services.AddSingleton<ILogService, LogService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IBrowserUtils, BrowserUtils>();
        builder.Services.AddSingleton<EmailVerificationHandler>();

        // 注册视图和视图模型
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        return app;
    }
}
