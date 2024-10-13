namespace API.DTOs.CohortDTOs
{
    public class CohortDetailDto : CohortDto
    {

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public List<string> CandidateNames { get; set; } = new();
    }
}
