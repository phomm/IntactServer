# OneSignal Integration for Intact Server

This document describes the OneSignal push notification integration for the Intact Server application.

## Overview

OneSignal is integrated as both a standalone notification service and as a wrapper around the email notification system, allowing push notifications to be sent as an alternative or supplement to traditional emails.

## Features

- ✅ **Push Notifications**: Send notifications to users via OneSignal
- ✅ **Email Wrapper**: Use OneSignal as an email service implementation
- ✅ **Segmentation**: Send to specific users, segments, or broadcast to all
- ✅ **Device Management**: Register and manage user devices
- ✅ **Tag-based Targeting**: Use tags for advanced user segmentation
- ✅ **Fallback Support**: Optional fallback to SMTP email when OneSignal fails

## Configuration

### 1. OneSignal Setup

1. Create an account at [OneSignal](https://onesignal.com/)
2. Create a new app in the OneSignal dashboard
3. Obtain your **App ID** and **REST API Key** from Settings → Keys & IDs

### 2. Application Configuration

Update `appsettings.json` with your OneSignal credentials:

```json
{
  "OneSignalSettings": {
    "AppId": "your-onesignal-app-id",
    "RestApiKey": "your-rest-api-key",
    "UserAuthKey": "optional-user-auth-key",
    "Enabled": true,
    "DefaultIconUrl": "https://yoursite.com/icon.png",
    "DefaultBadgeUrl": "https://yoursite.com/badge.png",
    "DefaultSound": "default",
    "TimeoutSeconds": 30
  }
}
```

**Configuration Options:**
- `AppId` (required): Your OneSignal App ID
- `RestApiKey` (required): Your OneSignal REST API Key
- `UserAuthKey` (optional): User-level authentication key for advanced operations
- `Enabled`: Enable/disable OneSignal notifications (default: false)
- `DefaultIconUrl`: Default notification icon URL
- `DefaultBadgeUrl`: Default notification badge URL
- `DefaultSound`: Default notification sound
- `TimeoutSeconds`: API request timeout (default: 30)

### 3. Service Registration

In `Program.cs`, register OneSignal services:

#### Option A: Standalone Notification Service
```csharp
builder.Services.AddOneSignalServices(builder.Configuration);
```

#### Option B: As Email Service Replacement (with fallback)
```csharp
builder.Services.AddOneSignalWithEmail(builder.Configuration, useFallback: true);
```

#### Option C: As Email Service Replacement (no fallback)
```csharp
builder.Services.AddOneSignalWithEmail(builder.Configuration, useFallback: false);
```

## Usage

### 1. Direct Notification Service

Inject `IOneSignalNotificationService` into your services:

```csharp
public class MyService
{
    private readonly IOneSignalNotificationService _notificationService;

    public MyService(IOneSignalNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task NotifyUser(string userId)
    {
        await _notificationService.SendToUserAsync(
            userId,
            "Welcome!",
            "Thank you for registering",
            new Dictionary<string, object> { { "action", "welcome" } }
        );
    }
}
```

### 2. As Email Service

When using `AddOneSignalWithEmail()`, the standard ASP.NET Identity `IEmailSender` interface will use OneSignal:

```csharp
public class AccountService
{
    private readonly IEmailSender _emailSender;

    public AccountService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendWelcomeEmail(string email)
    {
        // This will send via OneSignal if configured
        await _emailSender.SendEmailAsync(
            email,
            "Welcome to Intact",
            "<h1>Welcome!</h1><p>Thanks for joining.</p>"
        );
    }
}
```

## API Endpoints

The `OneSignalTestController` provides test endpoints:

### Get Configuration
```
GET /api/onesignaltest/config
```

### Send to User
```
POST /api/onesignaltest/send-to-user
Content-Type: application/json

{
  "userId": "user123",
  "title": "Test Notification",
  "message": "Hello from Intact Server!",
  "data": {
    "action": "test",
    "value": "123"
  }
}
```

### Broadcast to All Users
```
POST /api/onesignaltest/broadcast
Content-Type: application/json

{
  "title": "Server Announcement",
  "message": "New features available!",
  "data": {}
}
```

### Register Device
```
POST /api/onesignaltest/register-device
Content-Type: application/json

{
  "userId": "user123",
  "deviceToken": "device-token-from-client",
  "deviceType": "Android"
}
```

### Send Email Notification
```
POST /api/onesignaltest/send-email-notification
Content-Type: application/json

{
  "userId": "user123",
  "subject": "Account Verification",
  "htmlBody": "<p>Please verify your account.</p>"
}
```

### Update User Tags
```
POST /api/onesignaltest/update-tags
Content-Type: application/json

{
  "userId": "user123",
  "tags": {
    "premium": "true",
    "level": "gold"
  }
}
```

## Service Methods

### IOneSignalNotificationService

```csharp
// Send to single user
Task<bool> SendToUserAsync(string userId, string title, string message, 
    Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

// Send to multiple users
Task<bool> SendToUsersAsync(IEnumerable<string> userIds, string title, string message, 
    Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

// Send to segment (tag-based)
Task<bool> SendToSegmentAsync(Dictionary<string, string> tags, string title, string message, 
    Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

// Broadcast to all
Task<bool> SendBroadcastAsync(string title, string message, 
    Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

// Send email-style notification
Task<bool> SendEmailNotificationAsync(string userId, string subject, string htmlBody, 
    CancellationToken cancellationToken = default);

// Register device
Task<bool> RegisterUserDeviceAsync(string userId, string deviceToken, 
    string deviceType = "Android", CancellationToken cancellationToken = default);

// Update tags
Task<bool> UpdateUserTagsAsync(string userId, Dictionary<string, string> tags, 
    CancellationToken cancellationToken = default);
```

## Device Types

Supported device types for registration:
- `ios` - iOS devices
- `android` - Android devices
- `amazon` - Amazon devices
- `windowsphone` - Windows Phone
- `chrome` - Chrome browser
- `firefox` - Firefox browser
- `safari` - Safari browser
- `macos` - macOS devices
- `huawei` - Huawei devices
- `email` - Email notifications

## Architecture

### Components

1. **OneSignalSettings** - Configuration options class
2. **IOneSignalNotificationService** - Service interface
3. **OneSignalNotificationService** - Implementation using OneSignal .NET SDK
4. **OneSignalEmailService** - IEmailSender wrapper for Identity integration
5. **OneSignalExtensions** - DI registration extensions
6. **OneSignalTestController** - Testing API endpoints

### Integration Flow

```
User Action → Controller/Service
    ↓
IEmailSender (Identity) or IOneSignalNotificationService
    ↓
OneSignalEmailService (wrapper) or OneSignalNotificationService (direct)
    ↓
OneSignal .NET SDK
    ↓
OneSignal API
    ↓
User's Device (Push Notification)
```

## Best Practices

1. **User ID Mapping**: Store OneSignal Player IDs when devices register for better tag management
2. **Fallback Strategy**: Use email fallback for critical notifications
3. **Rate Limiting**: Be aware of OneSignal API rate limits
4. **Testing**: Use the test controller endpoints during development
5. **Monitoring**: Log all notification attempts for debugging
6. **Security**: Keep your REST API Key secure (never expose to clients)

## Troubleshooting

### Notifications Not Sending

1. Verify `Enabled: true` in configuration
2. Check that `AppId` and `RestApiKey` are correct
3. Ensure devices are properly registered
4. Check application logs for errors

### Fallback Not Working

1. Verify fallback email service is configured
2. Check `useFallback: true` in service registration
3. Ensure EmailSettings are properly configured

## NuGet Package

This integration uses the official OneSignal .NET SDK:
- **Package**: `OneSignalApi`
- **Version**: `5.0.0-alpha01`
- **Repository**: https://github.com/OneSignal/OneSignal-DotNet-SDK

## Additional Resources

- [OneSignal Documentation](https://documentation.onesignal.com/)
- [OneSignal .NET SDK](https://github.com/OneSignal/OneSignal-DotNet-SDK)
- [OneSignal REST API Reference](https://documentation.onesignal.com/reference)

## License

This integration follows the same license as the Intact Server project.
