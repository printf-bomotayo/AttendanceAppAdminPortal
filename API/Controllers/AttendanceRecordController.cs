using API.Entities;
using API.Services.AttendanceRecordService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class AttendanceRecordController : BaseApiController
    {
        private readonly IAttendanceRecordService _attendanceRecordService;
        public AttendanceRecordController(IAttendanceRecordService attendanceRecordService)
        {
            _attendanceRecordService = attendanceRecordService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceRecord>> GetById(int id)
        {
            var attendanceRecord = await _attendanceRecordService.GetByIdAsync(id);
            if (attendanceRecord == null)
            {
                return NotFound();
            }
            return Ok(attendanceRecord);
        }

        [HttpGet]
        public async Task<ActionResult<List<AttendanceRecord>>> GetAll()
        {
            var attendanceRecords = await _attendanceRecordService.GetAllAsync();
            return Ok(attendanceRecords);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AttendanceRecord attendanceRecord)
        {
            await _attendanceRecordService.AddAsync(attendanceRecord);
            return CreatedAtAction(nameof(GetById), new { id = attendanceRecord.Id }, attendanceRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AttendanceRecord attendanceRecord)
        {
            if (id != attendanceRecord.Id)
            {
                return BadRequest();
            }

            await _attendanceRecordService.UpdateAsync(attendanceRecord);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceRecordService.DeleteAsync(id);
            return NoContent();
        }
    }
}