using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CohortDTOs;
using API.DTOs.TrainingProgramDTOs;
using API.Entities;

namespace API.Services.TrainingProgramService
{
    public interface ITrainingProgramService
    {
        Task<TrainingProgramDto> GetByIdAsync(int trainingProgramId);
        Task<List<TrainingProgramDto>> GetAllAsync();
        Task<TrainingProgram> CreateTrainingProgramAsync(TrainingProgramDto trainingProgramDto);
        Task<TrainingProgram> UpdateAsync(int trainingProgramId, TrainingProgramUpdateDto trainingProgramUpdateDto);
        Task DeleteAsync(int trainingProgramId);
        Task AddCohortToTrainingProgramAsync(int trainingProgramId, CohortCreateDto cohortCreateDto);
    }
}