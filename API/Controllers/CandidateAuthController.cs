using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs.AuthenticationDTOs;
using API.DTOs.CandidateDTOs;
using API.Services.CandidateAuthService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CandidateAuthController : BaseApiController
    {

        private readonly ICandidateAuthService _candidateAuthService;
        public CandidateAuthController(ICandidateAuthService candidateAuthService)
        {
            _candidateAuthService = candidateAuthService;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> Signup(CandidateSignUpDto signupDto)
        {
            if (!_candidateAuthService.IsValidEmailDomain(signupDto.Email))
            {
                return BadRequest("Invalid email.");
            }

            var token = await _candidateAuthService.Signup(signupDto);
            return Ok(new { Token = token });

         
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CandidateLoginDto loginDto)
        {
            var token = await _candidateAuthService.Login(loginDto);
            return Ok(new { Token = token });
        
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _candidateAuthService.GenerateVerificationCodeAsync(dto.Email);
            return Ok("Verification code sent to your email.");
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto dto)
        {
            await _candidateAuthService.VerifyCodeAsync(dto.Email, dto.Code);
            return Ok("Verification successful. You can now reset your password.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
        {
            await _candidateAuthService.ResetPasswordAsync(dto);
            return Ok("Password reset successful.");
            

        }


        // [HttpPost("reset-password")]
        // public async Task<IActionResult> ResetPassword(CandidatePasswordResetDto resetDto)
        // {
        //     try
        //     {
        //         await _candidateAuthService.ResetPassword(resetDto);
        //         return Ok("Password reset successful");
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }
    }
}