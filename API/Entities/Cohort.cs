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
        public ActivityStatus Status { get; set; }
        public List<Candidate> CandidatesList { get; set; }
        public int TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
        // Administrative Details
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
            

        public enum ActivityStatus
        {
            Active,
            Pending,
            Completed
        }

        public void AddCandidate(Candidate candidate)
        {
            // Ensure the candidate's CohortId matches the current cohort's Id
            if (candidate.CohortId != this.Id)
            {
                throw new InvalidOperationException("The candidate's CohortId does not match the Cohort's Id.");
            }

            // Check if the cohort is active
            if (!IsActive())
            {
                throw new InvalidOperationException("Candidates can only be added to active cohorts.");
            }

            // Check if the candidate is already in the list based on a unique property (e.g., Email)
            if (CandidatesList.Any(c => c.Email == candidate.Email))
            {
                throw new InvalidOperationException("The candidate is already in the cohort.");
            }

            CandidatesList.Add(candidate);
        }

        public void RemoveCandidate(Candidate candidate)
        {
            // Check if the candidate is in the list
            var existingCandidate = CandidatesList.FirstOrDefault(c => c.Email == candidate.Email);

            if (existingCandidate == null)
            {
                throw new InvalidOperationException("The candidate is not in the cohort.");
            }

            CandidatesList.Remove(existingCandidate);
        }

        private bool IsActive()
        {
            return Status == ActivityStatus.Active;
        }

        // set Cohort status
        public void SetStatus(ActivityStatus status)
        {
            Status = status;
        }

    }
}