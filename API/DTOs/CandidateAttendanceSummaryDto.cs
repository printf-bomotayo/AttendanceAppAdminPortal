namespace API.DTOs
{
    public class CandidateAttendanceSummaryDto
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string StaffId { get; set; }

        // Aggregated attendance details
        public int TotalAppearances { get; set; } // Total days candidate marked attendance
        public double AttendancePercentage { get; set; } // Percentage appearance based on presence
        

        // Additional properties if needed
        public int TotalDays { get; set; } // Total days of the training program so far
    }

}
