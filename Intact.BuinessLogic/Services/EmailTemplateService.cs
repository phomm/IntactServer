namespace Intact.BusinessLogic.Services;

public interface IEmailTemplateService
{
    string GetEmailConfirmationTemplate(string userName, string confirmationLink);
    string GetPasswordResetTemplate(string userName, string resetLink);
    string GetEmailConfirmationSuccessPage();
    string GetEmailConfirmationErrorPage();
}

public class EmailTemplateService : IEmailTemplateService
{
    public string GetEmailConfirmationTemplate(string userName, string confirmationLink)
    {
        return $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2c3e50; text-align: center;'>Welcome to Intact!</h2>
                    <p>Hello {userName},</p>
                    <p>Thank you for registering with Intact Application. To complete your registration, please confirm your email address by clicking the link below:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{confirmationLink}' 
                           style='background-color: #3498db; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Confirm Email Address
                        </a>
                    </div>
                    <p>If you cannot click the button above, copy and paste the following link into your browser:</p>
                    <p style='word-break: break-all; color: #7f8c8d;'>{confirmationLink}</p>
                    <p>If you did not create this account, please ignore this email.</p>
                    <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                    <p style='color: #7f8c8d; font-size: 12px; text-align: center;'>
                        This is an automated message from Intact Application. Please do not reply to this email.
                    </p>
                </div>
            </body>
            </html>";
    }

    public string GetPasswordResetTemplate(string userName, string resetLink)
    {
        return $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #e74c3c; text-align: center;'>Password Reset Request</h2>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset your password for your Intact Application account. Click the link below to reset your password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #e74c3c; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Reset Password
                        </a>
                    </div>
                    <p>If you cannot click the button above, copy and paste the following link into your browser:</p>
                    <p style='word-break: break-all; color: #7f8c8d;'>{resetLink}</p>
                    <p>If you did not request this password reset, please ignore this email. Your password will not be changed.</p>
                    <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                    <p style='color: #7f8c8d; font-size: 12px; text-align: center;'>
                        This is an automated message from Intact Application. Please do not reply to this email.
                    </p>
                </div>
            </body>
            </html>";
    }

    public string GetPasswordResetCodeTemplate(string userName, string resetCode)
    {
        return $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #e74c3c; text-align: center;'>Password Reset Code</h2>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset your password for your Intact Application account. Use the following code to reset your password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <div style='background-color: #f8f9fa; border: 2px dashed #e74c3c; padding: 20px; border-radius: 5px; display: inline-block;'>
                            <span style='font-size: 24px; font-weight: bold; color: #e74c3c; letter-spacing: 3px;'>{resetCode}</span>
                        </div>
                    </div>
                    <p>Enter this code in the password reset form to continue.</p>
                    <p>If you did not request this password reset, please ignore this email. Your password will not be changed.</p>
                    <p><strong>Note:</strong> This code will expire in 15 minutes for security reasons.</p>
                    <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                    <p style='color: #7f8c8d; font-size: 12px; text-align: center;'>
                        This is an automated message from Intact Application. Please do not reply to this email.
                    </p>
                </div>
            </body>
            </html>";
    }

    public string GetEmailConfirmationSuccessPage()
    {
        return @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Email Confirmed - Intact</title>
                <style>
                    body { font-family: Arial, sans-serif; text-align: center; margin: 50px; }
                    .success { color: #27ae60; }
                    .container { max-width: 500px; margin: 0 auto; }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1 class='success'>✓ Email Confirmed!</h1>
                    <p>Your email has been successfully confirmed. You can now sign in to your account.</p>
                    <a href='/' style='background-color: #3498db; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Go to Application</a>
                </div>
            </body>
            </html>";
    }

    public string GetEmailConfirmationErrorPage()
    {
        return @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Email Confirmation Error - Intact</title>
                <style>
                    body { font-family: Arial, sans-serif; text-align: center; margin: 50px; }
                    .error { color: #e74c3c; }
                    .container { max-width: 500px; margin: 0 auto; }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1 class='error'>✗ Email Confirmation Failed</h1>
                    <p>There was an error confirming your email. The link may have expired or already been used.</p>
                    <a href='/' style='background-color: #3498db; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Go to Application</a>
                </div>
            </body>
            </html>";
    }
}