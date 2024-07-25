using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repository.CohortRepo
{
    public interface ICohortRepository
    {
        Task<Cohort> GetByIdAsync(int id);
        Task<List<Cohort>> GetAllAsync();
        Task AddAsync(Cohort cohort);
        Task UpdateAsync(Cohort cohort);
        Task DeleteAsync(int id);
        Task<Cohort> GetCohortByIdAsync(int id);
        Task UpdateCohortAsync(Cohort cohort);
    }
}