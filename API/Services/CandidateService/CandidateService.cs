using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Repository.CandidateRepo;

namespace API.Services.Implementations
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;

        }
        public async Task<CandidateDetailDto> GetCandidateByIdAsync(int id)
        {
            return await _candidateRepository.GetCandidateByIdAsync(id);
        }

        public async Task<CandidateAttendanceDto> GetCandidateByIdForAttendanceAsync(int id)
        {
            return await _candidateRepository.GetCandidateByIdForAttendanceAsync(id);
        }

        public async Task<CandidateDetailDto> GetCandidateByStaffIdAsync(string staffId)
        {
            return await _candidateRepository.GetCandidateByStaffIdAsync(staffId);
        }

        public async Task<List<CandidateDto>> GetCandidatesByCohortAsync(int cohortId)
        {
            return await _candidateRepository.GetCandidatesByCohortAsync(cohortId);
        }

        public async  Task<List<CandidateDto>> GetCandidatesByDepartmentAsync(string department)
        {
            return await _candidateRepository.GetCandidatesByDepartmentAsync(department);
        }

        public async Task<List<CandidateDto>> GetCandidatesByGenderAsync(string gender)
        {
            return await _candidateRepository.GetCandidatesByGenderAsync(gender);
        }

        public async Task<List<CandidateDto>> GetCandidatesByNameAsync(string name)
        {
            return await _candidateRepository.GetCandidatesByNameAsync(name);
        }

        public async Task<List<CandidateDto>> GetCandidatesAsync()
        {
            return await _candidateRepository.GetCandidatesAsync();
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _candidateRepository.AddAsync(candidate);
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            await _candidateRepository.UpdateAsync(candidate);
        }

        public async Task DeleteAsync(int id)
        {
           await _candidateRepository.DeleteAsync(id);
        }
    }
}