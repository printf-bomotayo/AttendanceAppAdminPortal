using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Cohort
    {
        // Cohort Details
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public List<Candidate> CandidatesList { get; set; }
        public int TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; }

        // Administrative Details
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
            

    }
}