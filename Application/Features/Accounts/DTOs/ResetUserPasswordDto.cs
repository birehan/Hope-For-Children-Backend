namespace Application.Features.Accounts.DTOs
{
    public class ResetUserPasswordDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}