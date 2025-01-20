namespace CursorAutoFree.Core.Models;

public class LicenseVerificationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string LicenseType { get; set; }
} 