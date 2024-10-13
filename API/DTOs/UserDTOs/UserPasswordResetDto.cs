namespace API.DTOs.UserDTOs
{
    public class UserPasswordResetDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
}
