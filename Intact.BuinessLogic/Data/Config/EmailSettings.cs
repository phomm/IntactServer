namespace Intact.BusinessLogic.Data.Config;

public class EmailSettings
{
    // SMTP Configuration (fallback)
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
    
    // Apprise Configuration (preferred)
    public string AppriseUrl { get; set; } = string.Empty;
    public bool UseApprise { get; set; } = true;
}