using API.DTOs.CandidateDTOs;
using API.DTOs.CohortDTOs;
using API.Entities;
using API.Services.CohortService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CohortController : BaseApiController
    {
        private readonly ICohortService _cohortService;
        private readonly IMapper _mapper;
        public CohortController(ICohortService cohortService, IMapper mapper)
        {
            _cohortService = cohortService;
            _mapper = mapper;

        }

        // Get all cohorts
        [HttpGet]
        public async Task<ActionResult<List<CohortDto>>> GetAllCohorts()
        {
            var cohorts = await _cohortService.GetAllCohortsAsync();
            return Ok(cohorts);
        }


        // POST: Create a new cohort
        [HttpPost]
        public async Task<IActionResult> CreateCohort([FromBody] CohortCreateDto cohortCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCohort = await _cohortService.CreateCohortAsync(cohortCreateDto);
            return CreatedAtAction(nameof(GetCohortById), new { id = createdCohort.Id }, createdCohort);
        }

        // PUT: Update an existing cohort
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCohort(int id, [FromBody] CohortUpdateDto cohortUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCohort = await _cohortService.UpdateCohortAsync(id, cohortUpdateDto);
                return Ok(updatedCohort);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: Get cohort by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCohortById(int id)
        {
            var cohort = await _cohortService.GetCohortByIdAsync(id);
            if (cohort == null)
            {
                return NotFound();
            }
            return Ok(cohort);
        }



        // POST: Add a candidate to a cohort
        [HttpPost("{cohortId}/add-candidate")]
        public async Task<IActionResult> AddCandidateToCohort(int cohortId, [FromBody] CandidateDto candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the DTO to the Candidate entity
            var candidate = _mapper.Map<Candidate>(candidateDto);

            try
            {
                await _cohortService.AddCandidateToCohortAsync(cohortId, candidate);
                return Ok(new { message = "Candidate successfully added to the cohort." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}