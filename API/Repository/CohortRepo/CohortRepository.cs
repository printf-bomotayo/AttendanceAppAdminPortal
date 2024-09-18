using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.CohortRepo
{
    public class CohortRepository : ICohortRepository
    {
        private readonly AppDbContext _context;
        public CohortRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<Cohort> GetCohortByIdAsync(int id)
        {
            return await _context.Cohorts
                                .Include(c => c.CandidatesList)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCohortAsync(Cohort cohort)
        {
            _context.Cohorts.Update(cohort);
            await _context.SaveChangesAsync();
        }
        
        public async Task<Cohort> GetByIdAsync(int id)
        {
            return await _context.Cohorts.Include(c => c.CandidatesList).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Cohort>> GetAllAsync()
        {
            return await _context.Cohorts.Include(c => c.CandidatesList).ToListAsync();
        }

        public async Task AddAsync(Cohort cohort)
        {
            await _context.Cohorts.AddAsync(cohort);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cohort cohort)
        {
            _context.Cohorts.Update(cohort);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cohort = await GetByIdAsync(id);
            if (cohort != null)
            {
                _context.Cohorts.Remove(cohort);
                await _context.SaveChangesAsync();
            }
        }
    }
}