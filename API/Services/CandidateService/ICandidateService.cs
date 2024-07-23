using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services
{
    public interface ICandidateService
    {
        Task<Candidate> GetCandidateByIdAsync(int id);
        Task<Candidate> GetCandidateByStaffIdAsync(string staffId);
        Task<List<Candidate>> GetCandidatesAsync();
        Task<List<Candidate>> GetCandidatesByDepartmentAsync(string department);
        Task<List<Candidate>> GetCandidatesByGenderAsync(string gender);
        Task<List<Candidate>> GetCandidatesByNameAsync(string name);
        Task<List<Candidate>> GetCandidatesByCohortAsync(int cohortId);
    }
}