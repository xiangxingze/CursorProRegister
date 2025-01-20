using CursorAutoFree.Core.Services;
using CursorAutoFree.Infrastructure.Auth;
using CursorAutoFree.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using CursorAutoFree.Infrastructure.Browser;

namespace CursorAutoFree.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCursorAutoFreeInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ILogService, LogService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBrowserUtils, BrowserUtils>();
        return services;
    }
} 