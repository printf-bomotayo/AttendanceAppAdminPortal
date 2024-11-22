using API.DTOs;
using API.DTOs.AttendanceDTOs;
using API.DTOs.CandidateDTOs;
using API.Entities;
using API.Services;
using API.Services.AttendanceRecordService;
using API.Services.CandidateService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AttendanceRecordController : BaseApiController
    {
        private readonly IAttendanceRecordService _attendanceRecordService;
		private readonly ICandidateService _candidateService;
        public AttendanceRecordController(IAttendanceRecordService attendanceRecordService, ICandidateService candidateService)
        {
            _attendanceRecordService = attendanceRecordService;
			_candidateService = candidateService;
        }


        [HttpPost("candidates/{candidateId}/attendance/mark")]
        public async Task<IActionResult> MarkAttendance(int candidateId, [FromBody] AttendanceMarkDto attendanceMarkDto)
        {
            var candidate = await _candidateService.GetCandidateByIdForAttendanceAsync(candidateId);
            if (candidate == null) return NotFound("Candidate not found");

            // Verify location (could be based on a proximity check)
            if (!IsLocationValid(attendanceMarkDto.Latitude, attendanceMarkDto.Longitude))
            {
                return BadRequest("Invalid location");

            }

            try
            {
                // Initialize validation flags
                bool isFingerprintValid = false;
                bool isFaceRecognitionValid = false;

                // Check if fingerprint data is provided
                if (attendanceMarkDto.FingerprintData != null && candidate.FingerprintData != null)
                {
                    isFingerprintValid = VerifyFingerprint(candidate.FingerprintData, attendanceMarkDto.FingerprintData);
                }

                // Check if face recognition data is provided
                if (attendanceMarkDto.FaceRecognitionData != null && candidate.FaceRecognitionData != null)
                {
                    isFaceRecognitionValid = VerifyFaceRecognition(candidate.FaceRecognitionData, attendanceMarkDto.FaceRecognitionData);
                }

                // If neither of the biometric validations passed, return an error
                if (!isFingerprintValid && !isFaceRecognitionValid)
                {
                    return BadRequest("Invalid biometric data. Either fingerprint or face recognition must match.");
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error occurred during biometric validation");
                return StatusCode(500, $"An error occurred while verifying biometric data......{ex.Message}");
            }


            // threshold for lateness
            var lateTimeThreshold = new TimeSpan(8, 0, 0); // 8:00 AM
            var checkInTime = DateTime.UtcNow.TimeOfDay; // Current time of check-in

            // print the checkintime and late timespan to review and compare the format
            Console.WriteLine(checkInTime.ToString(), lateTimeThreshold.ToString());

            // determine attendance status
            var attendanceStatus = checkInTime > lateTimeThreshold
                ? AttendanceStatus.Late
                : AttendanceStatus.Early;


            // Create new attendance record if all checks pass
            var attendanceRecord = new AttendanceRecord
            {
                CandidateId = candidateId,
                Date = DateTime.UtcNow,
                Status = attendanceStatus,
                CheckInTime = DateTime.UtcNow,
                Location = attendanceMarkDto.Location,
                Latitude = attendanceMarkDto.Latitude,
                Longitude = attendanceMarkDto.Longitude
            };

            await _attendanceRecordService.AddAsync(attendanceRecord);
            return Ok(new { message = "Attendance marked successfully", status = attendanceStatus });
        }



        [HttpGet]
		public async Task<ActionResult<List<AttendanceRecordDto>>> GetAll()
		{
            var attendanceRecords = await _attendanceRecordService.GetAllAsync();

            if (attendanceRecords == null || !attendanceRecords.Any())
                return NotFound("No attendance records found.");

            var attendanceRecordDtos = attendanceRecords.Select(ar => new AttendanceRecordResponseDto
            {
                Date = ar.Date,
                Status = ar.Status,
                CheckInTime = ar.CheckInTime,
                CheckOutTime = ar.CheckOutTime,
                CandidateStaffId = ar.Candidate?.StaffId ?? "N/A",
                CandidateId = ar.CandidateId,
                CandidateName = ar.Candidate != null
                                ? $"{ar.Candidate.FirstName} {ar.Candidate.LastName}"
                                : "Unknown Candidate",
                CandidateEmail = ar.Candidate?.Email ?? "N/A",
                Location = ar.Location,
                Latitude = ar.Latitude,
                Longitude = ar.Longitude
            }).ToList();

            return Ok(attendanceRecordDtos);
        }



		[HttpGet("{id}")]
		public async Task<ActionResult<AttendanceRecordResponseDto>> GetById(int id)
		{
			var attendanceRecord = await _attendanceRecordService.GetByIdAsync(id);

			if (attendanceRecord == null)
			{
				return NotFound();
			}

			var attendanceRecordDto = new AttendanceRecordResponseDto
			{
				Id = attendanceRecord.Id,
				CandidateId = attendanceRecord.CandidateId,
				Date = attendanceRecord.Date,
				Status = attendanceRecord.Status,
				CheckInTime = attendanceRecord.CheckInTime,
				CheckOutTime = attendanceRecord.CheckOutTime,
				Location = attendanceRecord.Location,
				Latitude = attendanceRecord.Latitude,
				Longitude = attendanceRecord.Longitude
			};

			return Ok(attendanceRecordDto);
		}


		[HttpPost("candidates/{candidateId}/attendance")]
		public async Task<ActionResult> Add(int candidateId, AttendanceRecordDto attendanceRecordDto)
		{
			var attendanceRecord = new AttendanceRecord
			{
				CandidateId = candidateId,
				Date = attendanceRecordDto.Date,
				Status = attendanceRecordDto.Status,
				CheckInTime = attendanceRecordDto.CheckInTime,
				CheckOutTime = attendanceRecordDto.CheckOutTime,
				Location = attendanceRecordDto.Location,
				Latitude = attendanceRecordDto.Latitude,
				Longitude = attendanceRecordDto.Longitude
			};

			await _attendanceRecordService.AddAsync(attendanceRecord);
			return CreatedAtAction(nameof(GetById), new { id = attendanceRecord.Id }, attendanceRecord);
		}

 

		[HttpPut("{candidate_id}/{id}")]
		public async Task<IActionResult> Update(int candidate_id, int id, AttendanceRecordDto attendanceRecordDto)
		{
			var attendanceRecord = await _attendanceRecordService.GetByIdAsync(id);

			if (attendanceRecord == null || attendanceRecord.CandidateId != candidate_id)
			{
				return NotFound("Attendance record not found or candidate mismatch.");
			}

			// Update only the relevant fields from the DTO
			attendanceRecord.Date = attendanceRecordDto.Date;
			attendanceRecord.Status = attendanceRecordDto.Status;
			attendanceRecord.CheckInTime = attendanceRecordDto.CheckInTime;
			attendanceRecord.CheckOutTime = attendanceRecordDto.CheckOutTime;
			attendanceRecord.Location = attendanceRecordDto.Location;
			attendanceRecord.Latitude = attendanceRecordDto.Latitude;
			attendanceRecord.Longitude = attendanceRecordDto.Longitude;

			await _attendanceRecordService.UpdateAsync(attendanceRecord);

			return NoContent();
		}


        // endpoint to allow candidates fetch and view their attendance record in real time
        // same endpoint can also allow admin fetch attendance records for a particular candidate and verify their presence
		[HttpGet("{candidate_id}/attendance-records")]
		public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetAttendanceRecordsByCandidateId(int candidate_id)
		{
			var attendanceRecords = await _attendanceRecordService.GetByCandidateIdAsync(candidate_id);

            if (attendanceRecords == null || !attendanceRecords.Any())
            {
                return NotFound("No attendance records found for this candidate.");
            }

			return Ok(attendanceRecords);
		}


        [HttpGet("candidates/{candidateId}/attendance-summary")]
        public async Task<ActionResult<CandidateAttendanceSummaryDto>> GetCandidateAttendanceSummary(int candidateId, int cohortId)
        {
            var summary = await _attendanceRecordService.GetCandidateAttendanceSummaryAsync(candidateId, cohortId);

            if (summary == null) return NotFound();

            return Ok(summary);
        }


        [HttpGet("candidates/attendance-summary/{cohortId}")]
        public async Task<IActionResult> GetCandidateAttendanceSummaries(int cohortId)
        {
            var summaries = await _attendanceRecordService.GetAllCandidateAttendanceSummariesAsync(cohortId);
            return Ok(summaries);
           
        }


        [HttpGet("candidates/{candidateId}/attendance")]
        public async Task<ActionResult<List<AttendanceRecordResponseDto>>> GetAttendanceRecordsByDateRange(int candidateId, DateTime startDate, DateTime endDate)
        {
            // Fetch attendance records for the specified candidate within the date range
            var attendanceRecords = await _attendanceRecordService.GetAttendanceRecordsByDateRangeAsync(candidateId, startDate, endDate);

            // If no records found, return a NotFound response
            if (attendanceRecords == null || !attendanceRecords.Any())
            {
                return NotFound("No attendance records found for the given date range.");
            }

            // Map to DTOs
            var attendanceRecordDtos = attendanceRecords.Select(ar => new AttendanceRecordResponseDto
            {
                Date = ar.Date,
                Status = ar.Status,
                CheckInTime = ar.CheckInTime,
                CheckOutTime = ar.CheckOutTime,
                Location = ar.Location,
                Latitude = ar.Latitude,
                Longitude = ar.Longitude,
                CandidateName = $"{ar.CandidateName}",
                CandidateEmail = ar.CandidateEmail,
                CandidateStaffId = ar.CandidateStaffId
            }).ToList();

            // Return the filtered records
            return Ok(attendanceRecordDtos);
        }


        [HttpGet("filter")]
        public async Task<ActionResult<List<AttendanceRecordDto>>> FilterAttendanceRecords([FromQuery] string? name, [FromQuery] string? email, [FromQuery] string? staffId)
        {
            // Fetch filtered attendance records from the service
            var attendanceRecords = await _attendanceRecordService.FilterAttendanceRecordsAsync(name, email, staffId);

            // Return filtered results
            if (attendanceRecords == null || !attendanceRecords.Any())
            {
                return NotFound("No matching attendance records found.");
            }

            return Ok(attendanceRecords);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceRecordService.DeleteAsync(id);
            return NoContent();
        }


        private bool IsLocationValid(double candidateLatitude, double candidateLongitude)
        {
            // Coordinates for the reference location (Lat: 6.480230, Long: 3.362580)
            const double referenceLatitude = 6.480230;
            const double referenceLongitude = 3.362580;

            // Set the radius threshold (200 meters)
            const double radiusInMeters = 200;


            // Calculate the distance between the candidate's location and the reference point
            double distance = CalculateDistance(candidateLatitude, candidateLongitude, referenceLatitude, referenceLongitude);

            return distance <= radiusInMeters;
        }


        private bool VerifyFingerprint(byte[] storedFingerprint, byte[] providedFingerprint)
        {
            return storedFingerprint.SequenceEqual(providedFingerprint);
        }


        private bool VerifyFaceRecognition(byte[] storedFaceData, byte[] providedFaceData)
        {
            
            return storedFaceData.SequenceEqual(providedFaceData);
        }


        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Earth's radius in meters
            var lat1Rad = DegreesToRadians(lat1);
            var lon1Rad = DegreesToRadians(lon1);
            var lat2Rad = DegreesToRadians(lat2);
            var lon2Rad = DegreesToRadians(lon2);

            var dLat = lat2Rad - lat1Rad;
            var dLon = lon2Rad - lon1Rad;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c; // Distance in meters
            return distance;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

    }
}