using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services.AttendanceRecordService
{
    public interface IAttendanceRecordService
    {
        Task<AttendanceRecord> GetByIdAsync(int id);
        Task<List<AttendanceRecord>> GetAllAsync();
        Task AddAsync(AttendanceRecord attendanceRecord);
        Task UpdateAsync(AttendanceRecord attendanceRecord);
        Task DeleteAsync(int id);
    }
}