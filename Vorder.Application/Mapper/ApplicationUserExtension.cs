using Vorder.Application.DTOs.Authentication;
using Vorder.Infrastructure.Data;

namespace Vorder.Application.Mapper
{
    public static class ApplicationUserExtension
    {

        public static UserDto ToUserDto(this ApplicationUser user) =>
             new UserDto(user.Id, user.UserName, user.Email, user.PhoneNumber);
    }
}
