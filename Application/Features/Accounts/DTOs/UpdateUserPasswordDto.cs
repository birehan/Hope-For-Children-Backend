namespace Application.Features.Accounts.DTOs
{
    public class UpdateUserPasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}