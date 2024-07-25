using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services.TrainingProgramService
{
    public interface ITrainingProgramService
    {
        Task<TrainingProgram> GetByIdAsync(int id);
        Task<List<TrainingProgram>> GetAllAsync();
        Task AddAsync(TrainingProgram trainingProgram);
        Task UpdateAsync(TrainingProgram trainingProgram);
        Task DeleteAsync(int id);
        Task AddCohortToTrainingProgramAsync(int trainingProgramId, Cohort cohort);
    }
}