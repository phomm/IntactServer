using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpPost("register-with-confirmation")]
    public async Task<IActionResult> RegisterWithConfirmation([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User 
        { 
            UserName = request.Email, 
            Email = request.Email 
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            // Generate email confirmation token
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Auth",
                new { userId = user.Id, code = encodedCode },
                Request.Scheme) ?? $"{Request.Scheme}://{Request.Host}/api/confirmEmail?userId={user.Id}&code={encodedCode}";

            try
            {
                await _emailService.SendEmailConfirmationAsync(request.Email, user.UserName ?? request.Email, callbackUrl);
                
                return Ok(new
                {
                    message = "Registration successful! Please check your email to confirm your account.",
                    requireEmailConfirmation = true
                });
            }
            catch (Exception ex)
            {
                // If email sending fails, we should still inform the user but log the error
                // In production, you'd want to use proper logging
                return Ok(new
                {
                    message = "Registration successful! However, there was an issue sending the confirmation email. Please contact support.",
                    requireEmailConfirmation = true,
                    emailError = true
                });
            }
        }

        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
        {
            return BadRequest("Invalid email confirmation request.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

        if (result.Succeeded)
        {
            // Return HTML page for better user experience
            var html = @"
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
            
            return Content(html, "text/html");
        }

        var errorHtml = @"
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

        return Content(errorHtml, "text/html");
    }

    [HttpPost("check-email-status")]
    [Authorize]
    public async Task<IActionResult> CheckEmailStatus()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        return Ok(new
        {
            email = user.Email,
            isEmailConfirmed = isConfirmed
        });
    }
}

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}