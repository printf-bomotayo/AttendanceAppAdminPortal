using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User : IdentityUser
    {
		public string VerificationCode { get; set; }
		public DateTime VerificationCodeExpiry { get; set; }
	}
}