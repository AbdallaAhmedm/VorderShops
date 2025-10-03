using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using System.Text;
using Vorder.Application.DTOs.Authentication;
using Vorder.Application.Interfaces.Services.Email;
using Vorder.Application.Mapper;
using Vorder.Application.ResultPattern;
using Vorder.Domain.Models;
using Vorder.Infrastructure.Data;
using Vorder.Infrastructure.Data.Constants;

namespace Vorder.WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AuthenticationController(UserManager<ApplicationUser> userManager,
                                    IEmailService emailSender) : ControllerBase
    {
        [HttpPost(Name = "Register")]
        public async Task<ApplicationResult<UserDto>> Register(RegisterDto model)
        {
            if (model.Email is null)
                return Errors.ValidationError();
            model.Email = model.Email.ToLower();

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
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

            string emailBody = EmailConstants.GetEmailBody(token);
            EmailMessage emailMessage = new EmailMessage("Confirm your email"
                , emailBody
                , model.Email);

            await emailSender.SendEmail(emailMessage);

            return user.ToUserDto();
        }

        [HttpGet(Name = "ConfirmEmail")]
        public async Task<ApplicationResult<string>> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Errors.NotFound(ErrorConstants.USERNOTFOUND, ErrorConstants.USERNOTFOUNDCODE);

            var result = await userManager.ConfirmEmailAsync(user, token);
            var error = result.Errors.FirstOrDefault();
            return result.Succeeded
                ? "Email confirmed! You can now log in."
                : Errors.ValidationError(error.Description, error.Code);
        }
    }
}
