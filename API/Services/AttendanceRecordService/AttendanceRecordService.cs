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