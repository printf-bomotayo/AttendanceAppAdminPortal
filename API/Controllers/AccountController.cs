using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs.AuthenticationDTOs;
using API.DTOs.UserDTOs;
using API.Entities;
using API.Services;
using API.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {

        private readonly UserManager<User> _userManager;

        private readonly TokenService _tokenService;

        private readonly AppDbContext _context;

        private readonly IEmailService _emailService;

        private readonly List<string> _validEmailDomains;
        public AccountController(UserManager<User> userManager, TokenService tokenService, AppDbContext context, IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
            _validEmailDomains = configuration.GetSection("ValidEmailDomains").Get<List<string>>();

        }

        private async Task<bool> VerifyCodeAsync(string email, string verificationCode)
        {
            var user = await _context.Users.SingleOrDefaultAsync(c => c.Email == email);

            if (user == null)
                throw new Exception("User not found.");

            if (user.VerificationCode != verificationCode || user.VerificationCodeExpiry < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired verification code.");
            }

            user.VerificationCode = null;
            user.VerificationCodeExpiry = DateTime.MinValue;

            await _context.SaveChangesAsync();
            return true;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName)
                        ?? await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user)
            };
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201);
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user == null) return BadRequest("Email does not exist");

            // genration verification code
            var verificationCode = new Random().Next(100000, 999999).ToString();

            user.VerificationCode = verificationCode;

            user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(20);

            // generate token for user authentication
            var userToken = await _tokenService.GenerateToken(user);

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code and token", $"Your verification code is {verificationCode}");

            return Ok("Verification code sent to your email.");
        }


        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(VerifyCodeDto verifyCodeDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(c => c.Email == verifyCodeDto.Email);

            if (user == null) throw new Exception("User not found.");

            if (user.VerificationCode != verifyCodeDto.Code || user.VerificationCodeExpiry < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired verification code.");
            }

            // Reset verification code after successful verification
            user.VerificationCode = null;

            user.VerificationCodeExpiry = DateTime.MinValue;

            await _context.SaveChangesAsync();

            return Ok("Code verified. You can now reset your password.");
        }





        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserPasswordResetDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null) return BadRequest("User not found");

            // Generate a verification code
            var verificationCode = GenerateVerificationCode();

            // Store the verification code in the user's record (assuming you have a method or property for this)
            user.VerificationCode = verificationCode;
            user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();

            // Generate the password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // use the generated token and code
            var isCodeValid = await VerifyCodeAsync(resetPasswordDto.Email, verificationCode);

            if (!isCodeValid) return BadRequest("Invalid or expired code");

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDto.NewPassword);

            if (!resetPasswordResult.Succeeded)
                return BadRequest("Password reset failed");

            return Ok("Password reset successful.");
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user)
            };
        }


        // utility method to generate random 6-digit code for verification
        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }


}