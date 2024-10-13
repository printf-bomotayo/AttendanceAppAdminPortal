using static API.Entities.Cohort;

namespace API.DTOs.CohortDTOs
{
    public class CohortDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string TrainingProgramName { get; set; }
        public int TotalCandidates { get; set; }
    }
}
