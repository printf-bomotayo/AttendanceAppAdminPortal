using API.Entities;

namespace API.DTOs
{
    public class CandidateDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender CandidateGender { get; set; }
    }
}
