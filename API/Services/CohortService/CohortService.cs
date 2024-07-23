using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repository.CohortRepo;

namespace API.Services.CohortService
{
    public class CohortService : ICohortService
    {
        private readonly ICohortRepository _cohortRepository;
        public CohortService(ICohortRepository cohortRepository)
        {
            _cohortRepository = cohortRepository;
        }

        public async Task<Cohort> GetByIdAsync(int id)
        {
            return await _cohortRepository.GetByIdAsync(id);
        }

        public async Task<List<Cohort>> GetAllAsync()
        {
            return await _cohortRepository.GetAllAsync();
        }

        public async Task AddAsync(Cohort cohort)
        {
            await _cohortRepository.AddAsync(cohort);
        }

        public async Task UpdateAsync(Cohort cohort)
        {
            await _cohortRepository.UpdateAsync(cohort);
        }

        public async Task DeleteAsync(int id)
        {
            await _cohortRepository.DeleteAsync(id);
        }
    }
}