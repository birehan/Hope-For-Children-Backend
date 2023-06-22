using System.ComponentModel.DataAnnotations;

namespace Application.Features.Accounts.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }

        public string UserRole { get; set; }

    }
}