# 📧 Apprise Email Architecture - .NET Identity Integration

## 🏗️ **Architecture Overview**

The email system now follows the proper .NET Identity workflow using `IEmailSender<User>` interface with Apprise as the notification backend and SMTP fallback.

### **Service Hierarchy**

```
EmailSender (IEmailSender<User>)
    ↓ Injects
EmailService (IEmailService) 
    ↓ Injects
AppriseEmailService (IAppriseEmailService) + EmailTemplateService
    ↓ Uses
NApprise Client + SMTP Fallback
```

## 🔧 **Core Components**

### **1. EmailSender** 
- **Interface**: `IEmailSender<User>` (Microsoft.AspNetCore.Identity)
- **Purpose**: .NET Identity integration for user-specific email operations
- **Methods**:
  - `SendConfirmationLinkAsync(User user, string email, string confirmationLink)`
  - `SendPasswordResetLinkAsync(User user, string email, string resetLink)`
  - `SendPasswordResetCodeAsync(User user, string email, string resetCode)`

### **2. EmailService**
- **Interface**: `IEmailService` (Custom)
- **Purpose**: High-level email operations with templating
- **Methods**:
  - `SendEmailConfirmationAsync(string email, string userName, string confirmationLink)`
  - `SendPasswordResetAsync(string email, string userName, string resetLink)`
  - `SendPasswordResetCodeAsync(string email, string userName, string resetCode)`

### **3. AppriseEmailService**
- **Interface**: `IAppriseEmailService` (Custom)
- **Purpose**: Low-level email delivery with Apprise integration
- **Features**:
  - ✅ NApprise client integration
  - ✅ Multiple Apprise URL support
  - ✅ Automatic SMTP fallback
  - ✅ Built-in mailto URL generation
  - ✅ Comprehensive logging

### **4. EmailTemplateService**
- **Interface**: `IEmailTemplateService` (Custom)
- **Purpose**: HTML email template generation
- **Templates**:
  - Email confirmation with beautiful styling
  - Password reset link with security warnings
  - Password reset code with highlighted display
  - Success/Error pages for email confirmation

## ⚙️ **Configuration**

### **EmailSettings Properties**

```json
{
  "EmailSettings": {
    // SMTP Configuration (Required)
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-app@gmail.com",
    "SenderName": "Your Application",
    "Username": "your-app@gmail.com",
    "Password": "your-app-password",
    "EnableSsl": true,
    
    // Apprise Configuration (Optional)
    "UseApprise": true,
    "AppriseUrls": [
      "discord://webhook_id/webhook_token",
      "slack://token_a/token_b/token_c/channel"
    ],
    "FallbackToSmtp": true
  }
}
```

### **Configuration Options**

| Setting | Type | Description |
|---------|------|-------------|
| `UseApprise` | bool | Enable/disable Apprise notifications |
| `AppriseUrls` | string[] | Custom Apprise notification URLs |
| `FallbackToSmtp` | bool | Fallback to SMTP if Apprise fails |

## 🔄 **Email Flow**

### **1. Registration Email Confirmation**

```csharp
// .NET Identity calls this automatically
await emailSender.SendConfirmationLinkAsync(user, email, confirmationLink);
    ↓
await emailService.SendEmailConfirmationAsync(email, userName, confirmationLink);
    ↓
await appriseEmailService.SendEmailAsync(email, subject, htmlTemplate);
    ↓
// Try Apprise first, fallback to SMTP if needed
```

### **2. Password Reset**

```csharp
// For reset links
await emailSender.SendPasswordResetLinkAsync(user, email, resetLink);

// For reset codes  
await emailSender.SendPasswordResetCodeAsync(user, email, resetCode);
```

## 🎨 **Email Templates**

### **Features**
- 📱 **Responsive Design**: Works on all devices
- 🎨 **Professional Styling**: Clean, modern appearance
- 🔒 **Security Messages**: Clear warnings and instructions
- 🔗 **Fallback Links**: Copy-paste options for broken buttons
- ✨ **Branded**: Consistent with your application

### **Template Types**
1. **Email Confirmation**: Welcome message with confirmation button
2. **Password Reset Link**: Security-focused reset instructions
3. **Password Reset Code**: Highlighted code display
4. **Success Pages**: Confirmation result pages

## 🧪 **Development Testing**

### **Available Test Endpoints**

```bash
# Test basic email
POST /api/emailtest/send-test
{
  "to": "test@example.com",
  "subject": "Test Subject",
  "body": "Test Body"
}

# Test email confirmation
POST /api/emailtest/test-confirmation
{
  "email": "user@example.com",
  "userName": "TestUser",
  "confirmationLink": "https://yourapp.com/confirm"
}

# Test password reset link
POST /api/emailtest/test-password-reset
{
  "email": "user@example.com", 
  "userName": "TestUser",
  "resetLink": "https://yourapp.com/reset"
}

# Test password reset code
POST /api/emailtest/test-password-reset-code
{
  "email": "user@example.com",
  "userName": "TestUser", 
  "resetCode": "ABC123"
}

# Get configuration info
GET /api/emailtest/config
```

## 🔧 **Service Registration**

```csharp
// In Program.cs or ServiceExtensions
services.AddTransient<IAppriseEmailService, AppriseEmailService>();
services.AddTransient<IEmailService, EmailService>();
services.AddTransient<IEmailTemplateService, EmailTemplateService>();
services.AddTransient<IEmailSender<User>, EmailSender>();
```

## 📦 **Dependencies**

```xml
<PackageReference Include="NApprise" Version="1.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="8.0.0" />
```

## 🚀 **Benefits**

### **✅ Proper .NET Identity Integration**
- Implements `IEmailSender<User>` for seamless identity workflow
- Automatic email confirmation and password reset support
- User context available for personalized emails

### **✅ Robust Delivery**
- Multiple notification backends via Apprise
- Automatic SMTP fallback for reliability
- Comprehensive error handling and logging

### **✅ Developer Friendly**
- Complete testing endpoints for development
- Beautiful HTML templates out of the box
- Flexible configuration options

### **✅ Production Ready**
- Environment-based endpoint security
- Comprehensive logging for debugging
- Scalable service architecture

## 🛠️ **Customization**

### **Adding Custom Templates**
1. Add method to `IEmailTemplateService`
2. Implement HTML template in `EmailTemplateService`
3. Add method to `IEmailService` to use the template
4. Call from `EmailSender` or controllers

### **Adding More Notification Channels**
1. Add Apprise URLs to `AppriseUrls` configuration
2. Apprise supports: Discord, Slack, Telegram, SMS, and many more
3. Each URL will receive the same notification

### **Custom Email Logic**
- Override `AppriseEmailService` for custom sending logic
- Implement `IAppriseEmailService` for completely different backends
- Add middleware for email auditing/tracking

## 🔐 **Security Considerations**

- ✅ App passwords for Gmail (not regular passwords)
- ✅ Environment variables for sensitive settings
- ✅ Development-only testing endpoints
- ✅ Proper error handling without exposing internals
- ✅ HTML sanitization in templates

The architecture is now fully compliant with .NET Identity standards while providing the flexibility and reliability of Apprise for email delivery!