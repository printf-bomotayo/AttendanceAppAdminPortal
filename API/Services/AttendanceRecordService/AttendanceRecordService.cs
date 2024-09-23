using API.DTOs;
using API.Entities;
using API.Repository.AttendanceRecordRepo;

namespace API.Services.AttendanceRecordService
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        public AttendanceRecordService(IAttendanceRecordRepository attendanceRecordRepository)
        {
             _attendanceRecordRepository = attendanceRecordRepository;
        }

        public async Task<AttendanceRecord> GetByIdAsync(int id)
        {
            return await _attendanceRecordRepository.GetByIdAsync(id);
        }

        public async Task<List<AttendanceRecord>> GetAllAsync()
        {
            return await _attendanceRecordRepository.GetAllAsync();
        }

		public async Task<IEnumerable<AttendanceRecord>> GetByCandidateIdAsync(int candidateId)
        {
            return await _attendanceRecordRepository.GetByCandidateIdAsync(candidateId);
        }

        public async Task<List<AttendanceRecordResponseDto>> FilterAttendanceRecordsAsync(string? name, string? email, string? staffId)
        {
            return await _attendanceRecordRepository.FilterAttendanceRecordsAsync(name, email, staffId);
        }

        public async Task<List<AttendanceRecordResponseDto>> GetAttendanceRecordsByDateRangeAsync(int candidateId, DateTime startDate, DateTime endDate)
        {
            return await _attendanceRecordRepository.GetAttendanceRecordsByDateRangeAsync(candidateId, startDate, endDate);
        }

        public async Task<CandidateAttendanceSummaryDto> GetCandidateAttendanceSummaryAsync(int candidateId, int cohortId)
        {
            return await _attendanceRecordRepository.GetCandidateAttendanceSummaryAsync(candidateId, cohortId);
        }

        public async Task<List<CandidateAttendanceSummaryDto>> GetAllCandidateAttendanceSummariesAsync(int cohortId)
        {
            return await _attendanceRecordRepository.GetAllCandidateAttendanceSummariesAsync(cohortId);
        }

        public async Task AddAsync(AttendanceRecord attendanceRecord)
        {
            await _attendanceRecordRepository.AddAsync(attendanceRecord);
        }

        public async Task UpdateAsync(AttendanceRecord attendanceRecord)
        {
            await _attendanceRecordRepository.UpdateAsync(attendanceRecord);
        }
              
        public async Task DeleteAsync(int id)
        {
            await _attendanceRecordRepository.DeleteAsync(id);
        }
            
    }
}