namespace API.DTOs.AttendanceDTOs
{
    public class AttendanceMarkDto
    {

        public byte[] FingerprintData { get; set; }
        public byte[] FaceRecognitionData { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }

    }
}
