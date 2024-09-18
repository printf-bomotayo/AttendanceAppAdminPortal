using API.Entities;
using API.Services.TrainingProgramService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TrainingProgramController : BaseApiController
    {
        private readonly ITrainingProgramService _trainingProgramService;
        public TrainingProgramController(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;

        }
        
        [HttpPost("{trainingProgramId}/cohorts")]
        public async Task<IActionResult> AddCohortToTrainingProgram(int trainingProgramId, [FromBody] Cohort cohort)
        {
            try
            {
                await _trainingProgramService.AddCohortToTrainingProgramAsync(trainingProgramId, cohort);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Training Program not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgram>> GetById(int id)
        {
            var trainingProgram = await _trainingProgramService.GetByIdAsync(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }
            return Ok(trainingProgram);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetAll()
        {
            var trainingPrograms = await _trainingProgramService.GetAllAsync();
            return Ok(trainingPrograms);
        }

        [HttpPost]
        public async Task<ActionResult> Add(TrainingProgram trainingProgram)
        {
            await _trainingProgramService.AddAsync(trainingProgram);
            return CreatedAtAction(nameof(GetById), new { id = trainingProgram.Id }, trainingProgram);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TrainingProgram trainingProgram)
        {
            if (id != trainingProgram.Id)
            {
                return BadRequest();
            }

            await _trainingProgramService.UpdateAsync(trainingProgram);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainingProgramService.DeleteAsync(id);
            return NoContent();
        }
    }
}