using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
// using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace API.Repository.AttendanceRecordRepo
{
    public class AttendanceRecordRepository : IAttendanceRecordRepository
    {
        private readonly AppDbContext _context;
        public AttendanceRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceRecord> GetByIdAsync(int id)
        {
            return await _context.AttendanceRecords.Include(ar => ar.Candidate).FirstOrDefaultAsync(ar => ar.Id == id);
        }

        /* 
         * public async Task<List<AttendanceRecord>> GetAllAsync()
        {
            return await _context.AttendanceRecords.Include(ar => ar.Candidate).ToListAsync();
        }
        */

		public async Task<List<AttendanceRecord>> GetAllAsync()
		{
			return await _context.AttendanceRecords.ToListAsync();
		}

		public async Task AddAsync(AttendanceRecord attendanceRecord)
        {
            try
            {
                await _context.AttendanceRecords.AddAsync(attendanceRecord);
                await _context.SaveChangesAsync();
            }
            
            catch (DbUpdateException ex)
            {
                // Log the detailed error message
                var mySqlException = ex.GetBaseException() as MySqlException;
                if (mySqlException != null)
                {
                    Console.WriteLine($"MySQL Error {mySqlException.Number}: {mySqlException.Message}");
                }
                throw;
            }
        }

		public async Task<IEnumerable<AttendanceRecord>> GetByCandidateIdAsync(int candidateId)
		{
			return await _context.AttendanceRecords
				.Where(ar => ar.CandidateId == candidateId)
				.ToListAsync();
		}


		public async Task UpdateAsync(AttendanceRecord attendanceRecord)
        {
            _context.AttendanceRecords.Update(attendanceRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var attendanceRecord = await GetByIdAsync(id);
            if (attendanceRecord != null)
            {
                _context.AttendanceRecords.Remove(attendanceRecord);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task<List<AttendanceRecordResponseDto>> FilterAttendanceRecordsAsync(string? name, string? email, string? staffId)
        {
            // Base query to fetch attendance records
            IQueryable<AttendanceRecord> query = _context.AttendanceRecords.Include(a => a.Candidate);

            // Apply filters based on provided parameters
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.Candidate.FirstName.Contains(name) || a.Candidate.LastName.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(a => a.Candidate.Email == email);
            }

            if (!string.IsNullOrWhiteSpace(staffId))
            {
                query = query.Where(a => a.Candidate.StaffId == staffId);
            }

            // Fetch the filtered list
            var filteredRecords = await query.ToListAsync();


            // Map the attendance records to AttendanceRecordResponseDto
            var attendanceRecordResponseDtos = filteredRecords.Select(ar => new AttendanceRecordResponseDto
            {
                Date = ar.Date,
                Status = ar.Status,
                CheckInTime = ar.CheckInTime,
                CheckOutTime = ar.CheckOutTime,
                Location = ar.Location,
                Latitude = ar.Latitude,
                Longitude = ar.Longitude,
                CandidateName = $"{ar.Candidate.FirstName} {ar.Candidate.LastName}",
                CandidateEmail = ar.Candidate.Email,
                CandidateStaffId = ar.Candidate.StaffId,
                CandidateId = ar.Candidate.Id
            }).ToList();

            return attendanceRecordResponseDtos ?? new List<AttendanceRecordResponseDto>();

        }


        public async Task<List<AttendanceRecordResponseDto>> GetAttendanceRecordsByDateRangeAsync(int candidateId, DateTime startDate, DateTime endDate)
        {
            // Fetch attendance records for the specified candidate within the date range
            var attendanceRecords = await _context.AttendanceRecords
                .Where(ar => ar.CandidateId == candidateId && ar.Date >= startDate && ar.Date <= endDate)
                .Include(ar => ar.Candidate)  // Including candidate details for the response
                .ToListAsync();

            var attendanceRecordDtos = attendanceRecords.Select(ar => new AttendanceRecordResponseDto
            {
                Id = ar.Id,
                CandidateId = ar.CandidateId,
                CandidateName = $"{ar.Candidate.FirstName} {ar.Candidate.LastName}",
                CandidateEmail = ar.Candidate.Email,
                CandidateStaffId = ar.Candidate.StaffId,
                Date = ar.Date,
                Status = ar.Status,
                CheckInTime = ar.CheckInTime,
                CheckOutTime = ar.CheckOutTime,
                Location = ar.Location,
                Latitude = ar.Latitude,
                Longitude = ar.Longitude
            }).ToList();

            return attendanceRecordDtos;
        }


        public async Task<CandidateAttendanceSummaryDto> GetCandidateAttendanceSummaryAsync(int candidateId, int cohortId)
        {
            var totalDaysInProgram = await _attendanceRecordRepository.GetTotalDaysForCohort(cohortId);
            var totalPresentDays = await _attendanceRecordService.GetTotalDaysPresentForCandidate(candidateId, cohortId);

            var candidate = await _candidateService.GetCandidateByIdAsync(candidateId);

            return new CandidateAttendanceSummaryDto
            {
                CandidateId = candidateId,
                CandidateName = $"{candidate.FirstName} {candidate.LastName}",
                CandidateEmail = candidate.Email,
                StaffId = candidate.StaffId,
                TotalAppearances = totalPresentDays,
                PercentageAppearances = (totalPresentDays / (double)totalDaysInProgram) * 100
            };
        }


        private async Task<int> GetTotalDaysForCohort(int cohortId)
        {
            // Logic to calculate the total days for the given cohort (training program duration)
            return await _context.AttendanceRecords.Where(r => r.CohortId == cohortId).Select(r => r.Date).Distinct().CountAsync();
        }

        private async Task<int> GetTotalDaysPresentForCandidate(int candidateId, int cohortId)
        {
            // Logic to calculate the total days the candidate was present (either Early or Late)
            return await _context.AttendanceRecords
                .Where(r => r.CandidateId == candidateId && r.CohortId == cohortId &&
                            (r.Status == AttendanceStatus.Early || r.Status == AttendanceStatus.Late))
                .Select(r => r.Date).Distinct().CountAsync();
        }



    }
}