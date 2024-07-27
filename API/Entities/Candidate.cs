namespace API.Entities
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffId { get; set; }
        public List<CandidatesGroup> Groups { get; set; }
        public string Department { get; set; }
        public int CohortId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<AttendanceRecord> AttendanceRecords { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

         // Fields for password reset
        public string VerificationCode { get; set; }
        public DateTime VerificationCodeExpiry { get; set; }

    }


    public enum Gender
    {
        Male,
        Female
    }
}