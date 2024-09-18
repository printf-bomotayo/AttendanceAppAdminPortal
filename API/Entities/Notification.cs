using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public int? CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public int? GroupId { get; set; }
        public CandidatesGroup Group { get; set; }
    }
}