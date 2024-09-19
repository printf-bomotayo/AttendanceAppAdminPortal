using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.DTOs;

namespace API.Repository.CandidateRepo
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly AppDbContext _context;
        public CandidateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CandidateDetailDto> GetCandidateByIdAsync(int id)
        {
            return await _context.Candidates
                .Where(c => c.Id == id)
                .Select(c => new CandidateDetailDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CandidateGender = c.CandidateGender,
                    PhoneNumber = c.PhoneNumber,
                    StaffId = c.StaffId,
                    Department = c.Department,
                    CohortId = c.CohortId,
                    Groups = c.Groups.Select(g => g.Name).ToList()
                }).FirstOrDefaultAsync();
        }


        public async Task<CandidateAttendanceDto> GetCandidateByIdForAttendanceAsync(int id)
        {
            return await _context.Candidates
                .Where(c => c.Id == id)
                .Select(c => new CandidateAttendanceDto
                {
                    Id = c.Id,
                    Email = c.Email,
                    FingerprintData = c.FingerprintData,
                    FaceRecognitionData = c.FaceRecognitionData
                }).FirstOrDefaultAsync();
        }

        public async Task<CandidateDetailDto> GetCandidateByStaffIdAsync(string staffId)
        {
            return await _context.Candidates
                .Where(c => c.StaffId == staffId)
                .Select(c => new CandidateDetailDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CandidateGender = c.CandidateGender,
                    PhoneNumber = c.PhoneNumber,
                    StaffId = c.StaffId,
                    Department = c.Department,
                    CohortId = c.CohortId,
                    Groups = c.Groups.Select(g => g.Name).ToList()
                }).FirstOrDefaultAsync();

        }

        public async  Task<List<CandidateDto>> GetCandidatesByCohortAsync(int cohortId)
        {
            return await _context.Candidates
                .Where(c => c.CohortId == cohortId)
                .Select(c => new CandidateDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CandidateGender = c.CandidateGender
                }).ToListAsync();
        }

        public async Task<List<CandidateDto>> GetCandidatesByDepartmentAsync(string department)
        {
            return await _context.Candidates
                .Where(c => c.Department == department)
                .Select(c => new CandidateDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CandidateGender = c.CandidateGender
                }).ToListAsync();
        }

        public async Task<List<CandidateDto>> GetCandidatesByGenderAsync(string gender)
        {
            return await _context.Candidates
                .Where(c => c.CandidateGender.ToString() == gender)
                .Select(c => new CandidateDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CandidateGender = c.CandidateGender
                }).ToListAsync();
        }

        public async Task<List<CandidateDto>> GetCandidatesByNameAsync(string name)
        {
            return await _context.Candidates
               .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
               .Select(c => new CandidateDto
               {
                   Id = c.Id,
                   FirstName = c.FirstName,
                   LastName = c.LastName,
                   Email = c.Email,
                   CandidateGender = c.CandidateGender
               }).ToListAsync();
        }

        public async Task<List<CandidateDto>> GetCandidatesAsync()
        {
            return await _context.Candidates
               .Select(c => new CandidateDto
               {
                   Id = c.Id,
                   FirstName = c.FirstName,
                   LastName = c.LastName,
                   Email = c.Email,
                   CandidateGender = c.CandidateGender
               }).ToListAsync();
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
        }
    }
}