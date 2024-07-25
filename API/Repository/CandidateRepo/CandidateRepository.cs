using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.CandidateRepo
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly AppDbContext _context;
        public CandidateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetCandidateByIdAsync(int id)
        {
            return await _context.Candidates.FindAsync(id);
        }

        public async Task<Candidate> GetCandidateByStaffIdAsync(string staffId)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.StaffId == staffId);
        }

        public async  Task<List<Candidate>> GetCandidatesByCohortAsync(int cohortId)
        {
            return await _context.Candidates.Where(c => c.CohortId == cohortId).ToListAsync();
        }

        public async Task<List<Candidate>> GetCandidatesByDepartmentAsync(string department)
        {
            return await _context.Candidates.Where(c => c.Department == department).ToListAsync();
        }

        public async Task<List<Candidate>> GetCandidatesByGenderAsync(string gender)
        {
            return await _context.Candidates.Where(c => c.Gender.ToString() == gender).ToListAsync();
        }

        public async Task<List<Candidate>> GetCandidatesByNameAsync(string name)
        {
            return await _context.Candidates.Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name)).ToListAsync();
        }

        public async Task<List<Candidate>> GetCandidatesAsync()
        {
            return await _context.Candidates.ToListAsync();
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