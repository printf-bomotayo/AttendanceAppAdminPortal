using API.Entities;

namespace API.DTOs.TrainingProgramDTOs
{
    public class TrainingProgramDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string FacilitatingInstitution { get; set; }
        public string Location { get; set; }
        public DateOnly StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public UnitOfDuration DurationUnit { get; set; }
        public int TotalTrainingDays { get; set; }
        

    }
}
