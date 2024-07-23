using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FacilitatingInstitution { get; set; }
         public string Location { get; set; }
        public int Duration { get; set; }
        public UnitOfDuration DurationUnit { get; set; }
        public List<Cohort> CohortList { get; set; } = new();
    }

    public enum UnitOfDuration
    {
        Week,
        Month,
        Year
    }
}