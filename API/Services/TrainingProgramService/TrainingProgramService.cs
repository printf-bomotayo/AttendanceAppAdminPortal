using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repository.TrainingProgramRepo;

namespace API.Services.TrainingProgramService
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        public TrainingProgramService(ITrainingProgramRepository trainingProgramRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
        }

        public async Task<TrainingProgram> GetByIdAsync(int id)
        {
            return await _trainingProgramRepository.GetByIdAsync(id);
        }

        public async Task<List<TrainingProgram>> GetAllAsync()
        {
            return await _trainingProgramRepository.GetAllAsync();
        }

        public async Task AddAsync(TrainingProgram trainingProgram)
        {
            await _trainingProgramRepository.AddAsync(trainingProgram);
        }

        public async Task UpdateAsync(TrainingProgram trainingProgram)
        {
            await _trainingProgramRepository.UpdateAsync(trainingProgram);
        }

        public async Task DeleteAsync(int id)
        {
            await _trainingProgramRepository.DeleteAsync(id);
        }
    }
}