using API.Entities;
using API.Services.CohortService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CohortController : BaseApiController
    {
        private readonly ICohortService _cohortService;
        public CohortController(ICohortService cohortService)
        {
            _cohortService = cohortService;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cohort>> GetById(int id)
        {
            var cohort = await _cohortService.GetByIdAsync(id);
            if (cohort == null)
            {
                return NotFound();
            }
            return Ok(cohort);
        }

        [HttpGet]
        public async Task<ActionResult<List<Cohort>>> GetAll()
        {
            var cohorts = await _cohortService.GetAllAsync();
            return Ok(cohorts);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Cohort cohort)
        {
            await _cohortService.AddAsync(cohort);
            return CreatedAtAction(nameof(GetById), new { id = cohort.Id }, cohort);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cohort cohort)
        {
            if (id != cohort.Id)
            {
                return BadRequest();
            }

            await _cohortService.UpdateAsync(cohort);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cohortService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{cohortId}/candidates")]
        public async Task<IActionResult> AddCandidateToCohort(int cohortId, [FromBody] Candidate candidate)
        {
            try
            {
                await _cohortService.AddCandidateToCohortAsync(cohortId, candidate);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Cohort not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}