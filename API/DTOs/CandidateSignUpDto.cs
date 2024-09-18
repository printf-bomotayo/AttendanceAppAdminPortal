namespace API.DTOs
{
    public class CandidateSignUpDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffId { get; set; }
        public string Department { get; set; }
        public int CohortId { get; set; }
    }
}