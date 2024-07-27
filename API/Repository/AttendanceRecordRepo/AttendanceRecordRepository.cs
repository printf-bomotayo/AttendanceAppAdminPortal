using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
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

        public async Task<List<AttendanceRecord>> GetAllAsync()
        {
            return await _context.AttendanceRecords.Include(ar => ar.Candidate).ToListAsync();
        }

        public async Task AddAsync(AttendanceRecord attendanceRecord)
        {
            try
            {
                await _context.AttendanceRecords.AddAsync(attendanceRecord);
                await _context.SaveChangesAsync();
            }
            // catch (DbUpdateException ex)
            // {
            //     // Log the detailed error message
            //     var sqlException = ex.GetBaseException() as SqliteException;
            //     if (sqlException != null)
            //     {
            //         Console.WriteLine($"SQLite Error {sqlException.SqliteErrorCode}: {sqlException.Message}");
            //     }
            //     throw;
            // }
            
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
    }
}