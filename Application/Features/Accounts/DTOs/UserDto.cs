using static Domain.AppUser;

namespace Application.Features.Accounts.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public string UserRole { get; set; }

    }
}