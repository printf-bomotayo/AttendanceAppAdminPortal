using API.DTOs.CohortDTOs;
using API.DTOs.TrainingProgramDTOs;
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

        // POST: Add a cohort to a training program
        [HttpPost("{trainingProgramId}/add-cohort")]
        public async Task<IActionResult> AddCohortToTrainingProgram(int trainingProgramId, [FromBody] CohortCreateDto cohortCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _trainingProgramService.AddCohortToTrainingProgramAsync(trainingProgramId, cohortCreateDto);
                return Ok(new { message = "Cohort successfully added to the training program." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<IActionResult> CreateTrainingProgram([FromBody] TrainingProgramDto trainingProgramDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trainingProgram = await _trainingProgramService.CreateTrainingProgramAsync(trainingProgramDto);
            return CreatedAtAction(nameof(GetById), new { id = trainingProgram.Id }, trainingProgram);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingProgram(int trainingProgramId, [FromBody] TrainingProgramUpdateDto trainingProgramUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedProgram = await _trainingProgramService.UpdateAsync(trainingProgramId, trainingProgramUpdateDto);
                return Ok(updatedProgram);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainingProgramService.DeleteAsync(id);
            return NoContent();
        }
    }
}