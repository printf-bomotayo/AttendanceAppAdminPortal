using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs.AuthenticationDTOs;
using API.DTOs.CandidateDTOs;
using API.Entities;
using API.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace API.Services.CandidateAuthService
{
    public class CandidateAuthService : ICandidateAuthService
    {
        private readonly AppDbContext _context;

        private readonly TokenService _tokenService;

#pragma warning disable CS0169 // The field 'CandidateAuthService._cache' is never used

        private readonly IDistributedCache _cache;

#pragma warning restore CS0169 // The field 'CandidateAuthService._cache' is never used

        private readonly IEmailService _emailService;

        private readonly List<string> _validEmailDomains;

        public CandidateAuthService(AppDbContext context, TokenService tokenService, IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _tokenService = tokenService;
            _context = context;
            _validEmailDomains = configuration.GetSection("ValidEmailDomains").Get<List<string>>();
        }

        public bool IsValidEmailDomain(string email)
        {
            var emailDomain =  email.Split('@').Last();
            return  _validEmailDomains.Contains(emailDomain);
        }

        public async Task<string> Signup(CandidateSignUpDto signupDto)
        {

            if (!IsValidEmailDomain(signupDto.Email))
            {
                throw new Exception("Invalid email domain.");
            }

            if (await _context.Candidates.AnyAsync(c => c.Email == signupDto.Email))
            {
                throw new Exception("Email is already registered.");
            }

            var candidate = new Candidate
            {
                FirstName = signupDto.FirstName,
                LastName = signupDto.LastName,
                Email = signupDto.Email,
                CandidateGender = signupDto.CandidateGender,
                PhoneNumber = signupDto.PhoneNumber,
                StaffId = signupDto.StaffId,
                Department = signupDto.Department,
                CohortId = signupDto.CohortId
            };

            using var hmac = new HMACSHA512();
            candidate.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signupDto.Password));
            candidate.PasswordSalt = hmac.Key;

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            await _emailService.SendCandidateRegistrationConfirmationAsync(candidate.Email, candidate.FirstName);

            return _tokenService.GenerateCandidateToken(candidate);
        }



         public async Task<string> Login(CandidateLoginDto loginDto)
        {
            var candidate = await _context.Candidates
                .SingleOrDefaultAsync(c => c.Email == loginDto.Email);

            if (candidate == null) throw new Exception("Invalid email");

            using var hmac = new HMACSHA512(candidate.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(candidate.PasswordHash)) throw new Exception("Invalid password");

            return _tokenService.GenerateCandidateToken(candidate);
        }

        


        public async Task ResetPasswordAsync(PasswordResetDto resetDto)
        {
            var candidate = await _context.Candidates
                .SingleOrDefaultAsync(c => c.Email == resetDto.Email);

            if (candidate == null) throw new Exception("Candidate not found");

            using var hmac = new HMACSHA512();

            candidate.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(resetDto.NewPassword));

            candidate.PasswordSalt = hmac.Key;

            _context.Candidates.Update(candidate);

            await _context.SaveChangesAsync();
        }


        public async Task GenerateVerificationCodeAsync(string email)
        {
            var candidate = await _context.Candidates.SingleOrDefaultAsync(c => c.Email == email);

            if (candidate == null) throw new Exception("Candidate not found.");

            var verificationCode = new Random().Next(100000, 999999).ToString();

            candidate.VerificationCode = verificationCode;

            candidate.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(candidate.Email, "Password Reset Verification Code", $"Your verification code is {verificationCode}.");
        }

        public async Task VerifyCodeAsync(string email, string verificationCode)
        {
            var candidate = await _context.Candidates.SingleOrDefaultAsync(c => c.Email == email);

            if (candidate == null) throw new Exception("Candidate not found.");

            if (candidate.VerificationCode != verificationCode || candidate.VerificationCodeExpiry < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired verification code.");
            }

            // Reset verification code after successful verification
            candidate.VerificationCode = null;

            candidate.VerificationCodeExpiry = DateTime.MinValue;

            await _context.SaveChangesAsync();
        }
    }
}