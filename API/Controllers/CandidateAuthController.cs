using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
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
            try
            {
                var token = await _candidateAuthService.Signup(signupDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CandidateLoginDto loginDto)
        {
            try
            {
                var token = await _candidateAuthService.Login(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(CandidatePasswordResetDto resetDto)
        {
            try
            {
                await _candidateAuthService.ResetPassword(resetDto);
                return Ok("Password reset successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}