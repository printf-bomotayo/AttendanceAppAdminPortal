using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.CandidateAuthService
{
    public interface ICandidateAuthService
    {
        Task<string> Signup(CandidateSignUpDto signupDto);
        Task<string> Login(CandidateLoginDto loginDto);
        Task ResetPasswordAsync(CandidatePasswordResetDto resetDto);
        Task GenerateVerificationCodeAsync(string email);
        Task VerifyCodeAsync(string email, string verificationCode);

    }
}