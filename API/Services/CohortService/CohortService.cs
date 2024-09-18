using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repository.CandidateRepo;
using API.Repository.CohortRepo;

namespace API.Services.CohortService
{
    public class CohortService : ICohortService
    {
        private readonly ICohortRepository _cohortRepository;
        private readonly ICandidateRepository _candidateRepository;
        public CohortService(ICohortRepository cohortRepository, ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
            _cohortRepository = cohortRepository;
        }

         public async Task<Cohort> GetCohortByIdAsync(int id)
        {
            return await _cohortRepository.GetCohortByIdAsync(id);
        }

        public async Task AddCandidateToCohortAsync(int cohortId, Candidate candidate)
        {
            var cohort = await _cohortRepository.GetCohortByIdAsync(cohortId);
            if (cohort == null)
            {
                throw new KeyNotFoundException("Cohort not found");
            }

            candidate.CohortId = cohortId;
            cohort.CandidatesList.Add(candidate);

            await _candidateRepository.AddAsync(candidate);
            await _cohortRepository.UpdateCohortAsync(cohort);
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