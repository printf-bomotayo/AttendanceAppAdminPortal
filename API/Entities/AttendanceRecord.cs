using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AttendanceRecord
    {
        public int Id {get; set;}
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Location { get; set; }  
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }

    public enum AttendanceStatus
    {
        Early,
        Absent,
        Late
    }
}