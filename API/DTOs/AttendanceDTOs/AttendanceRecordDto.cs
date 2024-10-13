using System;
using API.Entities;


// DTO to allow trainees/candidates view attendance records in real-time. 
namespace API.DTOs.AttendanceDTOs
{
    public class AttendanceRecordDto
    {
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public string TrainingProgramName { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
