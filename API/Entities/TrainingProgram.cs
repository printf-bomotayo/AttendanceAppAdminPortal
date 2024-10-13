using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FacilitatingInstitution { get; set; }
         public string Location { get; set; }
        public DateOnly StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public UnitOfDuration DurationUnit { get; set; }
        public List<Cohort> CohortList { get; set; }
        public int TotalTrainingDays {  get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public enum UnitOfDuration
    {
        Week,
        Month,
        Year
    }
}