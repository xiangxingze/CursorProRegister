namespace CursorAutoFree.Core.Models;

public class AuthInfo
{
    public string LicenseKey { get; set; }
    public bool IsAuthorized { get; set; }
    public int RemainingAttempts { get; set; }
    public DateTime LastUsed { get; set; }
    public DateTime? ExpiryDate { get; set; }
} 