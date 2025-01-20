namespace CursorAutoFree.Core.Services;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// 注册新账号
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <param name="password">密码</param>
    /// <returns>注册是否成功</returns>
    Task<bool> RegisterAccountAsync(string email, string password);

    /// <summary>
    /// 更新认证信息
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <param name="token">认证令牌</param>
    /// <returns>更新是否成功</returns>
    Task<bool> UpdateAuthAsync(string email, string token);

    /// <summary>
    /// 清除认证信息
    /// </summary>
    /// <returns>清除是否成功</returns>
    Task<bool> ClearAuthAsync();
} 