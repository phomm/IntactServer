# 📧 Apprise Email Integration

This branch implements **Apprise-based email notifications** using the [NApprise](https://www.nuget.org/packages/NApprise/) NuGet package for reliable email delivery in the Intact application.

## 🌟 Features

- ✅ **Multi-backend Support**: Gmail, Outlook, SMTP, and more
- ✅ **Email Confirmations**: Integrated with ASP.NET Core Identity
- ✅ **Password Reset Emails**: Automated password recovery
- ✅ **HTML Templates**: Beautiful, responsive email templates
- ✅ **Fallback Support**: Automatic fallback to standard SMTP
- ✅ **Development Testing**: Built-in test endpoints for development
- ✅ **Comprehensive Logging**: Detailed logging for troubleshooting

## 🚀 Quick Setup

### 1. Configure Email Settings

Update your `appsettings.json`:

```json
{
  "EmailSettings": {
    "UseApprise": true,
    "AppriseUrl": "mailto://your-username:your-password@smtp.gmail.com:587",
    "SenderEmail": "noreply@yourdomain.com",
    "SenderName": "Intact Application"
  }
}
```

### 2. Gmail Setup (Recommended)

For Gmail, use an **App Password** instead of your regular password:

1. Enable 2-Factor Authentication on your Gmail account
2. Go to [Google Account Settings](https://myaccount.google.com/security)
3. Generate an "App Password" for the application
4. Use the app password in your configuration:

```json
{
  "EmailSettings": {
    "UseApprise": true,
    "AppriseUrl": "mailto://your-email@gmail.com:your-app-password@smtp.gmail.com:587",
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Intact Application"
  }
}
```

### 3. Environment Variables (Production)

For production, use environment variables:

```bash
EMAIL_APPRISE_URL="mailto://username:password@smtp.example.com:587"
EMAIL_SENDER_EMAIL="noreply@yourdomain.com"
EMAIL_SENDER_NAME="Intact Production"
```

## 📋 Configuration Examples

See `apprise-email-config.example.json` for comprehensive configuration examples including:

- Gmail with App Password
- Outlook 365
- Custom SMTP servers
- Advanced SSL/TLS options
- Environment variable setup

## 🔧 Service Implementation

### AppriseEmailService

The main service (`AppriseEmailService.cs`) provides:

```csharp
public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink);
    Task SendPasswordResetAsync(string email, string userName, string resetLink);
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}
```

### Key Features:

- **Automatic URL Building**: Constructs Apprise-compatible URLs from settings
- **Error Handling**: Comprehensive error handling with logging
- **Security**: Proper URL encoding and validation
- **Flexibility**: Supports both Apprise URLs and SMTP fallback

## 🧪 Testing (Development Only)

The `EmailTestController` provides testing endpoints available in development:

### Test Basic Email
```bash
POST /api/emailtest/send-test
{
  "to": "test@example.com",
  "subject": "Test Subject",
  "body": "<h1>Test Email</h1>"
}
```

### Test Email Confirmation
```bash
POST /api/emailtest/test-confirmation
{
  "email": "user@example.com",
  "userName": "TestUser",
  "confirmationLink": "https://localhost:7000/confirmEmail?userId=123&code=abc"
}
```

### Test Password Reset
```bash
POST /api/emailtest/test-password-reset
{
  "email": "user@example.com",
  "userName": "TestUser",
  "resetLink": "https://localhost:7000/reset-password?token=xyz"
}
```

### Get Configuration Info
```bash
GET /api/emailtest/config
```

## 🔗 Integration with Identity

Email confirmation is automatically integrated with ASP.NET Core Identity:

1. **User Registration**: Sends confirmation email automatically
2. **Email Confirmation**: `/confirmEmail` endpoint handles verification
3. **Password Reset**: Integrated with Identity's password reset flow
4. **HTML Responses**: Beautiful success/error pages for confirmation

## 🛡️ Security Notes

- **Never commit credentials** to version control
- Use **App Passwords** for Gmail (not regular passwords)
- Use **environment variables** in production
- **Test thoroughly** before deploying
- Monitor **logs** for email delivery issues

## 📦 Dependencies

- **NApprise** (1.3.0): Core Apprise notification library
- **Microsoft.Extensions.Logging**: For comprehensive logging
- **Microsoft.Extensions.Options**: For configuration binding

## 🚀 Deployment

The service is automatically registered in the DI container:

```csharp
// In InternalServicesExtensions.cs
services.AddTransient<IEmailService, AppriseEmailService>();
```

## 🔍 Troubleshooting

### Common Issues:

1. **Gmail Authentication Errors**
   - Ensure 2FA is enabled
   - Use App Password, not regular password
   - Check Gmail security settings

2. **SMTP Connection Issues**
   - Verify SMTP server and port
   - Check SSL/TLS settings
   - Confirm firewall isn't blocking connections

3. **Apprise URL Format**
   - Ensure proper URL encoding
   - Check parameter syntax
   - Validate server details

### Logging:

Enable detailed logging in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Intact.BusinessLogic.Services": "Debug"
    }
  }
}
```

## 🎯 Next Steps

1. **Configure** your email settings
2. **Test** using development endpoints
3. **Deploy** to your target environment
4. **Monitor** logs for any issues
5. **Customize** email templates as needed

---

## 📚 Additional Resources

- [NApprise Documentation](https://github.com/bridgingit/napprise)
- [Apprise Project](https://github.com/caronc/apprise)
- [ASP.NET Core Identity Email Confirmation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm)

For questions or issues, check the application logs or create an issue in the repository.