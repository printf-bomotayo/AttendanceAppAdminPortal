using API.Entities;

namespace API.Services.CohortService
{
    public interface ICohortService
    {
        Task<Cohort> GetByIdAsync(int id);
        Task<List<Cohort>> GetAllAsync();
        Task AddAsync(Cohort cohort);
        Task UpdateAsync(Cohort cohort);
        Task DeleteAsync(int id);
        Task<Cohort> GetCohortByIdAsync(int id);
        Task AddCandidateToCohortAsync(int cohortId, Candidate candidate);

    }
}