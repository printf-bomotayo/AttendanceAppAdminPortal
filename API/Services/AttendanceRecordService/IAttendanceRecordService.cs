using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.AttendanceDTOs;
using API.DTOs.CandidateDTOs;
using API.Entities;

namespace API.Services.AttendanceRecordService
{
    public interface IAttendanceRecordService
    {
        Task<AttendanceRecord> GetByIdAsync(int id);

        Task<List<AttendanceRecord>> GetAllAsync();
        Task<List<AttendanceRecordResponseDto>> GetAllCandidatesRecordsAsync();

        Task AddAsync(AttendanceRecord attendanceRecord);

        // to fetch and view a trainee attendance records real time.
        Task<IEnumerable<AttendanceRecordDto>> GetByCandidateIdAsync(int candidateId);

        Task<List<AttendanceRecordResponseDto>> FilterAttendanceRecordsAsync(string name, string email, string staffId);

        Task<List<AttendanceRecordResponseDto>> GetAttendanceRecordsByDateRangeAsync(int candidateId, DateTime startDate, DateTime endDate);
        Task<List<AttendanceRecordResponseDto>> GetAttendanceRecordsByPunctualityStatus(string status);

        Task<CandidateAttendanceSummaryDto> GetCandidateAttendanceSummaryAsync(int candidateId, int cohortId);
        Task<List<CandidateAttendanceSummaryDto>> GetAllCandidateAttendanceSummariesAsync(int cohortId);

        Task UpdateAsync(AttendanceRecord attendanceRecord);

        Task DeleteAsync(int id);

        Task<byte[]> GenerateCsvAsync(IEnumerable<AttendanceRecordResponseDto> records);

        Task<byte[]> GeneratePdfAsync(IEnumerable<AttendanceRecordResponseDto> attendanceRecords);

        Task<byte[]> GenerateCsvAllRecords(List<CandidateAttendanceSummaryDto> allrecords);

    }
}