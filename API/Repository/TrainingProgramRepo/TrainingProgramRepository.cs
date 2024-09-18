using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.TrainingProgramRepo
{
    public class TrainingProgramRepository : ITrainingProgramRepository
    {
        private readonly AppDbContext _context;
        public TrainingProgramRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<TrainingProgram> GetByIdAsync(int id)
        {
            return await _context.TrainingPrograms.Include(tp => tp.CohortList).FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task<List<TrainingProgram>> GetAllAsync()
        {
            return await _context.TrainingPrograms.Include(tp => tp.CohortList).ToListAsync();
        }

        public async Task AddAsync(TrainingProgram trainingProgram)
        {
            await _context.TrainingPrograms.AddAsync(trainingProgram);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Update(trainingProgram);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trainingProgram = await GetByIdAsync(id);
            if (trainingProgram != null)
            {
                _context.TrainingPrograms.Remove(trainingProgram);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TrainingProgram> GetTrainingProgramByIdAsync(int id)
        {
            return await _context.TrainingPrograms
                                .Include(tp => tp.CohortList)
                                .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task UpdateTrainingProgramAsync(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Update(trainingProgram);
            await _context.SaveChangesAsync();
        }
    }
}