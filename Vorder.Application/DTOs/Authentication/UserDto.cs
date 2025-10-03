namespace Vorder.Application.DTOs.Authentication
{
    public record UserDto(Guid id, string Username, string Email, string PhoneNumber);
}
