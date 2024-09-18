using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.CandidateAuthService
{
    public interface ICandidateAuthService
    {
        Task<string> Signup(CandidateSignUpDto signupDto);
        Task<string> Login(CandidateLoginDto loginDto);
        Task ResetPasswordAsync(PasswordResetDto resetDto);
        Task GenerateVerificationCodeAsync(string email);
        Task VerifyCodeAsync(string email, string verificationCode);
        public bool IsValidEmailDomain(string email);

    }
}