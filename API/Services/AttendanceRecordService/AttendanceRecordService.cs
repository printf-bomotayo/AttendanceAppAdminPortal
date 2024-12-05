using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs.AttendanceDTOs;
using API.DTOs.CandidateDTOs;
using API.Entities;
using API.Services;
using API.Services.CandidateService;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iTextSharp.text;
// using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;

namespace API.Services.AttendanceRecordService
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly AppDbContext _context;
        private readonly ICandidateService _candidateService;
        public AttendanceRecordService(AppDbContext context, ICandidateService candidateService)
        {
            _context = context;
            _candidateService = candidateService;
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
            //return await _context.AttendanceRecords.ToListAsync();
            
            return await _context.AttendanceRecords
                    .Include(ar => ar.Candidate) // Ensure Candidate is included
                    .ToListAsync();
        }

        //added to get AttendanceRecordResponseDto return type
        public async Task<List<AttendanceRecordResponseDto>> GetAllCandidatesRecordsAsync()
        {
            var records = await _context.AttendanceRecords.Include(ar => ar.Candidate).ToListAsync();
            var attendanceRecordResponseDtos = records.Select(ar => new AttendanceRecordResponseDto
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
            return attendanceRecordResponseDtos;
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

        // Method To allow candidate view their attendnance records in real time 
        public async Task<IEnumerable<AttendanceRecordDto>> GetByCandidateIdAsync(int candidateId)
        {
            var attendanceRecords = await _context.AttendanceRecords.Where(ar => ar.CandidateId == candidateId)
                  .Include(ar => ar.Candidate)
                    .ThenInclude(c => c.Cohort)
                    .ThenInclude(cohort => cohort.TrainingProgram)
                    .ToListAsync();

            // Map to AttendanceRecordDto 
            var attendanceRecordDtos = attendanceRecords.Select(ar => new AttendanceRecordDto
            {
                TrainingProgramName = ar.Candidate.Cohort?.TrainingProgram?.Name,
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


        public async Task<List<AttendanceRecordResponseDto>> FilterAttendanceRecordsAsync(string name, string email, string staffId)
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

        public async Task<List<AttendanceRecordResponseDto>> GetAttendanceRecordsByPunctualityStatus(string status)
        {
            // Fetch attendance records for the specified candidates that are early, absent or late
            IQueryable<AttendanceRecord> query = _context.AttendanceRecords.Include(a => a.Candidate);

            if (!string.IsNullOrWhiteSpace(status))
            {
                //convert status to match Enum declaration
                bool conversion = Enum.TryParse<AttendanceStatus>(status, true, out var convertedStatus);
                if (!conversion)
                {
                    throw new ArgumentException("Invalid Attendance Status");
                }
                query = query.Where(a => a.Status == convertedStatus);

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
            return  new List<AttendanceRecordResponseDto>();

        }


        public async Task<CandidateAttendanceSummaryDto> GetCandidateAttendanceSummaryAsync(int candidateId, int cohortId)
        {
            var totalDaysInProgram = await GetTotalDaysForCohort(cohortId);
            var totalPresentDays = await GetTotalDaysPresentForCandidate(candidateId, cohortId);

            var candidate = await _candidateService.GetCandidateByIdAsync(candidateId);

            return new CandidateAttendanceSummaryDto
            {
                CandidateId = candidateId,
                CandidateName = $"{candidate.FirstName} {candidate.LastName}",
                CandidateEmail = candidate.Email,
                StaffId = candidate.StaffId,
                TotalAppearances = totalPresentDays,
                AttendancePercentage = totalPresentDays / (double)totalDaysInProgram * 100
            };
        }



        public async Task<List<CandidateAttendanceSummaryDto>> GetAllCandidateAttendanceSummariesAsync(int cohortId)
        {
            var candidates = await _context.Candidates
                .Where(c => c.CohortId == cohortId)
                .ToListAsync();

            var totalDaysForCohort = await GetTotalDaysForCohort(cohortId);

            var candidateSummaries = new List<CandidateAttendanceSummaryDto>();

            foreach (var candidate in candidates)
            {
                var totalDaysPresent = await GetTotalDaysPresentForCandidate(candidate.Id, cohortId);

                var percentageAppearance = totalDaysForCohort > 0
                    ? (double)totalDaysPresent / totalDaysForCohort * 100
                    : 0;

                var summaryDto = new CandidateAttendanceSummaryDto
                {
                    CandidateId = candidate.Id,
                    CandidateName = $"{candidate.FirstName} {candidate.LastName}",
                    StaffId = candidate.StaffId,
                    CandidateEmail = candidate.Email,
                    TotalAppearances = totalDaysPresent,
                    AttendancePercentage = percentageAppearance
                };

                candidateSummaries.Add(summaryDto);
            }

            return candidateSummaries;
        }



        public async Task<int> GetTotalDaysForCohort(int cohortId)
        {
            // Logic to calculate the total days for a given cohort (training program duration)
            return await _context.AttendanceRecords
                .Where(r => r.Candidate.CohortId == cohortId)  // Joining AttendanceRecords with Candidate to filter by cohortId
                .Select(r => r.Date)
                .Distinct()
                .CountAsync();
        }


        public async Task<int> GetTotalDaysPresentForCandidate(int candidateId, int cohortId)
        {
            // Logic to calculate the total days the candidate was present (either Early or Late)
            return await _context.AttendanceRecords
                .Where(r => r.CandidateId == candidateId && r.Candidate.CohortId == cohortId &&
                            (r.Status == AttendanceStatus.Early || r.Status == AttendanceStatus.Late))  // Filter based on attendance status
                .Select(r => r.Date)
                .Distinct()
                .CountAsync();
        }


        public async Task<byte[]> GenerateCsvAsync(IEnumerable<AttendanceRecordResponseDto> records)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Date,CandidateName,Status,CheckInTime,Location,CheckOutTime");

                foreach (var record in records)
                {
                    stringBuilder.AppendLine(
                        $"{record.Date:yyyy-MM-dd},{record.CandidateName},{record.Status},{record.CheckInTime},{record.Location}," +
                        $"{record.CheckOutTime}" //+
                        //$"\"{record.CandidateName}\",\"{record.CandidateEmail}\""
                    );
                }

                return await Task.FromResult(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
            }    
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured {ex.Message}");
                throw ex;
            }
        }

        public Task<byte[]> GenerateCsvAllRecords(List<CandidateAttendanceSummaryDto> allrecords)
        {
            try
            {
                var stringbuilder = new StringBuilder();
                stringbuilder.AppendLine("Candidate Name, Candidate Email, Total Appearance, Attendance Percentage");

                foreach (var record in allrecords)
                {
                    stringbuilder.AppendLine($"{record.CandidateName},{record.CandidateEmail},{record.TotalAppearances},{record.AttendancePercentage}");
                }
                return Task.FromResult(Encoding.UTF8.GetBytes(stringbuilder.ToString()));
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"An error occured {ex.Message}");
                throw ex;
            }

        }

        public async Task<byte[]> GeneratePdfAsync(IEnumerable<AttendanceRecordResponseDto> attendanceRecords)
        {
            using var memoryStream = new MemoryStream();                         
                
            try
            {
                using (var writer = new PdfWriter(memoryStream))
                using (var pdfDocument = new PdfDocument(writer))
                {
                    var document = new Document(pdfDocument);

                    // Add Title Section and Set Font
                    //var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var docfont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                    document.SetFont(docfont).SetFontSize(26);
                    document.Add(new Paragraph("CANDIDATE ATTENDANCE RECORD")).SetFontSize(14);
                    document.Add(new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}"));
                    document.Add(new Paragraph("\n"));

                    // Add Table
                    var table = new Table(UnitValue.CreatePercentArray(6)).UseAllAvailableWidth(); // 6 columns
                    table.AddHeaderCell("DATE");
                    table.AddHeaderCell("NAME");
                    table.AddHeaderCell("STATUS");
                    table.AddHeaderCell("CHECK-IN TIME");
                    table.AddHeaderCell("LOCATION");
                    table.AddHeaderCell("CHECk-OUT TIME");
                    //table.AddHeaderCell("LATITUDE");
                    //table.AddHeaderCell("LONGITUDE");
                    //table.AddHeaderCell("CANDIDATE NAME");

                    foreach (var record in attendanceRecords)
                    {
                        table.AddCell(record.Date.ToString("yyyy-MM-dd"));
                        table.AddCell(record.CandidateName.ToString());
                        table.AddCell(record.Status.ToString()); 
                        table.AddCell(record.CheckInTime?.ToString("HH:mm:ss") ?? "N/A");
                        table.AddCell(record.Location);
                        table.AddCell(record.CheckOutTime?.ToString("HH:mm:ss") ?? "N/A");
                        //table.AddCell(record.Latitude.ToString());
                        //table.AddCell(record.Longitude.ToString());
                        //table.AddCell(record.CandidateName);
                    }

                    document.Add(table);
                    document.Close();

                    //var generatedDocument = await Task.FromResult(memoryStream.ToArray());
                    //return generatedDocument;
                    return await Task.FromResult(memoryStream.ToArray());
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured {ex.Message}");
                return Array.Empty<byte>();
                //throw;
            }

        }       

    }
}