using API.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.DTOs.AttendanceDTOs
{
    public class AttendanceRecordResponseDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidateStaffId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

