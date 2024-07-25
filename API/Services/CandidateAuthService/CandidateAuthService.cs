using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CandidateAuthService
{
    public class CandidateAuthService : ICandidateAuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public CandidateAuthService(AppDbContext context, TokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
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

        public async Task<string> Signup(CandidateSignUpDto signupDto)
        {
            if (await _context.Candidates.AnyAsync(c => c.Email == signupDto.Email))
            {
                throw new Exception("Email is already registered.");
            }

            var candidate = new Candidate
            {
                FirstName = signupDto.FirstName,
                LastName = signupDto.LastName,
                Email = signupDto.Email,
                Gender = Enum.Parse<Gender>(signupDto.Gender, true),
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

            return _tokenService.GenerateCandidateToken(candidate);
        }
    }
}