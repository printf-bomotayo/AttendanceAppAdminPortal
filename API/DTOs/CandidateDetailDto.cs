namespace API.DTOs
{
    public class CandidateDetailDto : CandidateDto
    {
        public string PhoneNumber { get; set; }
        public string StaffId { get; set; }
        public string Department { get; set; }
        public int CohortId { get; set; }
        public List<string> Groups { get; set; } = new();
    }
}
