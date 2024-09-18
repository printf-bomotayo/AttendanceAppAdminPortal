using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Services
{
    public interface ICandidateService
    {
        Task<CandidateDetailDto> GetCandidateByIdAsync(int id);
        Task<CandidateAttendanceDto> GetCandidateByIdForAttendanceAsync(int id);
        Task<CandidateDetailDto> GetCandidateByStaffIdAsync(string staffId);
        Task<List<CandidateDto>> GetCandidatesAsync();
        Task<List<CandidateDto>> GetCandidatesByDepartmentAsync(string department);
        Task<List<CandidateDto>> GetCandidatesByGenderAsync(string gender);
        Task<List<CandidateDto>> GetCandidatesByNameAsync(string name);
        Task<List<CandidateDto>> GetCandidatesByCohortAsync(int cohortId);
        Task AddAsync(Candidate candidate);
        Task UpdateAsync(Candidate candidate);
        Task DeleteAsync(int id);
    }
}