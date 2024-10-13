using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CohortDTOs;
using API.Entities;

namespace API.Services.CohortService
{
    public interface ICohortService
    {
        Task<List<CohortDto>> GetAllCohortsAsync();
        Task<CohortDetailDto> GetCohortByIdAsync(int cohortId);
        Task<Cohort> CreateCohortAsync(CohortCreateDto cohortCreateDto);
        Task<Cohort> UpdateCohortAsync(int cohortId, CohortUpdateDto cohortUpdateDto);
        Task AddCandidateToCohortAsync(int cohortId, Candidate candidate);
        Task DeleteAsync(int cohortId);
    }

}