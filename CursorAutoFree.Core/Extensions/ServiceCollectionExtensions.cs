using CursorAutoFree.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CursorAutoFree.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCursorAutoFreeCore(this IServiceCollection services)
    {
        services.AddScoped<EmailVerificationHandler>();
        return services;
    }
} 