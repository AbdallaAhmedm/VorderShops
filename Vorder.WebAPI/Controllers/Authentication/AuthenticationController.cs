using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Vorder.Application.DTOs.Authentication;
using Vorder.Application.Interfaces.Services.Email;
using Vorder.Application.ResultPattern;
using Vorder.Domain.Models;
using Vorder.Infrastructure.Data;

namespace Vorder.WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AuthenticationController(UserManager<ApplicationUser> userManager,
                                    IEmailService emailSender) : ControllerBase
    {
        [HttpPost(Name = "Register")]
        public async Task<ApplicationResult<string>> Register(RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                IdentityError? error = result.Errors.FirstOrDefault();
                if (error is not null)
                    return Errors.ValidationError(error.Description, error.Code);
                else
                    return Errors.ValidationError();
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmationLink = $"{Request.Scheme}://{Request.Host}/Vorder/api/Authentication/ConfirmEmail?userId={user.Id}&token={encodedToken}";
            string emailBody = $@"
                    <p>Please confirm your account by clicking the button below:</p>
                    <a href='{confirmationLink}' 
                       style='display: inline-block; 
                              padding: 12px 24px; 
                              font-size: 16px; 
                              color: white; 
                              background-color: #007bff; 
                              text-decoration: none; 
                              border-radius: 6px;'>
                       Confirm Email
                    </a>";
            EmailMessage emailMessage = new EmailMessage("Confirm your email"
                , emailBody
                , model.Email);

            await emailSender.SendEmail(emailMessage);

            return "User registered. Please check your email to confirm your account.";
        }

        [HttpGet(Name = "ConfirmEmail")]
        public async Task<ApplicationResult<string>> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Errors.NotFound(ErrorConstants.USERNOTFOUND, ErrorConstants.USERNOTFOUNDCODE);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded
                ? "Email confirmed! You can now log in."
                : Errors.ValidationError(ErrorConstants.EMAILCONFIRMATIONFAILED, ErrorConstants.EMAILCONFIRMATIONFAILEDCODE);
        }
    }
}
