namespace API.DTOs
{
    public class CandidateAttendanceDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] FingerprintData { get; set; }
        public byte[] FaceRecognitionData { get; set; }
 
    }
}
