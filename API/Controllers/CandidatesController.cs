using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class CandidatesController : BaseApiController
    {
        private readonly ICandidateService _candidateService;
        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService;                        
        }

        [HttpGet]
        public async Task<ActionResult<List<Candidate>>> GetCandidates()
        {
            return await _candidateService.GetCandidatesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(int id)
        {
            var candidate = await  _candidateService.GetCandidateByIdAsync(id);

            if (candidate == null) return NotFound();

            return candidate;
        }

        [HttpGet("staffId/{staffId}")]
        public async Task<ActionResult<Candidate>> GetCandidateByStaffId(string staffId)
        {
            var candidate = await _candidateService.GetCandidateByStaffIdAsync(staffId);
            if (candidate == null)
            {
                return NotFound();
            }
            return Ok(candidate);
        }

        [HttpGet("department/{department}")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesByDepartment(string department)
        {
            var candidates = await _candidateService.GetCandidatesByDepartmentAsync(department);
            return Ok(candidates);
        }

        [HttpGet("gender/{gender}")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesByGender(string gender)
        {
            var candidates = await _candidateService.GetCandidatesByGenderAsync(gender);
            return Ok(candidates);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesByName(string name)
        {
            var candidates = await _candidateService.GetCandidatesByNameAsync(name);
            return Ok(candidates);
        }

        [HttpGet("cohort/{cohortId}")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesByCohort(int cohortId)
        {
            var candidates = await _candidateService.GetCandidatesByCohortAsync(cohortId);
            return Ok(candidates);
        }
    }
}