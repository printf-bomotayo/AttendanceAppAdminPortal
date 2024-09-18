using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repository.TrainingProgramRepo
{
    public interface ITrainingProgramRepository
    {
        Task<TrainingProgram> GetByIdAsync(int id);
        Task<List<TrainingProgram>> GetAllAsync();
        Task AddAsync(TrainingProgram trainingProgram);
        Task UpdateAsync(TrainingProgram trainingProgram);
        Task DeleteAsync(int id);
        Task<TrainingProgram> GetTrainingProgramByIdAsync(int id);
        Task UpdateTrainingProgramAsync(TrainingProgram trainingProgram);
    }
}