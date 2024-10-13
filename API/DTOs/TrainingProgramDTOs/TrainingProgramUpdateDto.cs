namespace API.DTOs.TrainingProgramDTOs
{
    public class TrainingProgramUpdateDto : TrainingProgramDto
    {
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
