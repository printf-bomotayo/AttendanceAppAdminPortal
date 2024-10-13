using API.Entities;
using static API.Entities.Cohort;

namespace API.DTOs.CohortDTOs
{
    public class CohortCreateDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int TrainingProgramId { get; set; }
        public List<int> CandidateIds { get; set; } // List of candidates to be added to the cohort
    }
}
