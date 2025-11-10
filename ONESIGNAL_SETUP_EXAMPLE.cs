// Example Program.cs setup for OneSignal Integration
// Add this to your Program.cs to enable OneSignal notifications

using Intact.API.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

// ... other service registrations ...

// ═══════════════════════════════════════════════════════════
// ONESIGNAL INTEGRATION - Choose one of the following options:
// ═══════════════════════════════════════════════════════════

// OPTION 1: Standalone OneSignal Notification Service
// Use this if you want OneSignal as an additional notification channel
// alongside your existing email service
builder.Services.AddOneSignalServices(builder.Configuration);

// OPTION 2: OneSignal as Primary Email Service with SMTP Fallback
// Use this to replace the default email service with OneSignal,
// but fall back to SMTP if OneSignal fails
// builder.Services.AddOneSignalWithEmail(builder.Configuration, useFallback: true);

// OPTION 3: OneSignal as Primary Email Service (No Fallback)
// Use this to completely replace email with push notifications
// builder.Services.AddOneSignalWithEmail(builder.Configuration, useFallback: false);

// ═══════════════════════════════════════════════════════════
// END ONESIGNAL INTEGRATION
// ═══════════════════════════════════════════════════════════

var app = builder.Build();

// ... rest of your Program.cs ...

app.Run();

/* 
 * CONFIGURATION REQUIRED:
 * 
 * Add to your appsettings.json:
 * 
 * {
 *   "OneSignalSettings": {
 *     "AppId": "your-onesignal-app-id",
 *     "RestApiKey": "your-rest-api-key",
 *     "UserAuthKey": "",
 *     "Enabled": true,
 *     "DefaultIconUrl": "https://yoursite.com/icon.png",
 *     "DefaultBadgeUrl": "",
 *     "DefaultSound": "default",
 *     "TimeoutSeconds": 30
 *   }
 * }
 * 
 * USAGE EXAMPLES:
 * 
 * 1. Direct Notification Service (Option 1):
 * 
 *    public class MyService
 *    {
 *        private readonly IOneSignalNotificationService _notifications;
 *        
 *        public MyService(IOneSignalNotificationService notifications)
 *        {
 *            _notifications = notifications;
 *        }
 *        
 *        public async Task NotifyUser(string userId)
 *        {
 *            await _notifications.SendToUserAsync(
 *                userId,
 *                "Welcome!",
 *                "Thanks for joining Intact Server"
 *            );
 *        }
 *    }
 * 
 * 2. Email Service Integration (Option 2 or 3):
 * 
 *    public class AccountService
 *    {
 *        private readonly IEmailSender _emailSender;
 *        
 *        public AccountService(IEmailSender emailSender)
 *        {
 *            _emailSender = emailSender;
 *        }
 *        
 *        public async Task SendVerification(string email)
 *        {
 *            // This will use OneSignal if configured
 *            await _emailSender.SendEmailAsync(
 *                email,
 *                "Verify your account",
 *                "<p>Click here to verify</p>"
 *            );
 *        }
 *    }
 * 
 * TEST ENDPOINTS:
 * 
 * - GET  /api/onesignaltest/config
 * - POST /api/onesignaltest/send-to-user
 * - POST /api/onesignaltest/broadcast
 * - POST /api/onesignaltest/register-device
 * - POST /api/onesignaltest/send-email-notification
 * - POST /api/onesignaltest/update-tags
 */
