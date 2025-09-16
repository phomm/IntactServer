namespace Intact.BusinessLogic.Data.Config;

public class EmailSettings
{
    // SMTP Configuration
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;

    // Apprise Configuration
    public bool UseApprise { get; set; } = true;
    public string[] AppriseUrls { get; set; } = Array.Empty<string>();
    public bool FallbackToSmtp { get; set; } = true;
}