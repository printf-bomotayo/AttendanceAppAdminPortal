using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs.CohortDTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CohortService
{
    public class CohortService : ICohortService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CohortService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // Create a new cohort
        public async Task<Cohort> CreateCohortAsync(CohortCreateDto cohortCreateDto)
        {
            var cohort = _mapper.Map<Cohort>(cohortCreateDto);

            // Set CreatedBy and CreatedDate
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            cohort.CreatedBy = currentUserId;
            cohort.CreatedDate = DateTime.UtcNow;

            cohort.ModifiedBy = currentUserId;
            cohort.ModifiedDate = DateTime.UtcNow;

            await _context.Cohorts.AddAsync(cohort);
            await _context.SaveChangesAsync();

            return cohort;
        }




        public async Task<Cohort> UpdateCohortAsync(int cohortId, CohortUpdateDto cohortUpdateDto)
        {
            var cohort = await _context.Cohorts.FindAsync(cohortId);

            if (cohort == null)
            {
                throw new KeyNotFoundException($"Cohort with ID {cohortId} not found.");
            }

            // Map updated fields from DTO
            _mapper.Map(cohortUpdateDto, cohort);

            // Set ModifiedBy and ModifiedDate
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            cohort.ModifiedBy = currentUserId;
            cohort.ModifiedDate = DateTime.UtcNow;

            _context.Cohorts.Update(cohort);
            await _context.SaveChangesAsync();

            return cohort;
        }



        // GET: Get cohort by ID
       




        // Get all cohorts with general details
        public async Task<List<CohortDto>> GetAllCohortsAsync()
        {
            var cohorts = await _context.Cohorts
                .Include(c => c.TrainingProgram)
                .Include(c => c.CandidatesList)
                .Select(c => new CohortDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.Status,
                    TrainingProgramName = c.TrainingProgram.Name,
                    TotalCandidates = c.CandidatesList.Count
                })
                .ToListAsync();

            return cohorts;
        }


        public async Task<CohortDetailDto> GetCohortByIdAsync(int cohortId)
        {
            var cohort = await _context.Cohorts
                .Include(c => c.TrainingProgram)
                .Include(c => c.CandidatesList)
                .FirstOrDefaultAsync(c => c.Id == cohortId);

            if (cohort == null) return null;

            return new CohortDetailDto
            {
                Name = cohort.Name,
                Description = cohort.Description,
                StartDate = cohort.StartDate,
                EndDate = cohort.EndDate,
                Status = cohort.Status,
                TrainingProgramName = cohort.TrainingProgram.Name,
                TotalCandidates = cohort.CandidatesList.Count,
                CreatedDate = cohort.CreatedDate,
                ModifiedDate = cohort.ModifiedDate,
                CreatedBy = cohort.CreatedBy,
                ModifiedBy = cohort.ModifiedBy,
                CandidateNames = cohort.CandidatesList.Select(c => $"{c.FirstName} {c.LastName}").ToList()
            };
        }


        public async Task DeleteAsync(int cohortId)
        {
            var cohort = await _context.Cohorts.Include(c => c.CandidatesList).FirstOrDefaultAsync(c => c.Id == cohortId);

            if (cohort != null)
            {
                _context.Cohorts.Remove(cohort);
                await _context.SaveChangesAsync();
            }
        }

        // Method to add a candidate to a cohort
        public async Task AddCandidateToCohortAsync(int cohortId, Candidate candidate)
        {
            // Check if the cohort exists
            var cohort = await _context.Cohorts
                .Include(c => c.CandidatesList)
                .FirstOrDefaultAsync(c => c.Id == cohortId);

            if (cohort == null)
            {
                throw new KeyNotFoundException($"Cohort with ID {cohortId} not found.");
            }

            // Check if the candidate is already in the cohort
            if (cohort.CandidatesList.Any(c => c.Id == candidate.Id))
            {
                throw new InvalidOperationException($"Candidate with ID {candidate.Id} is already in the cohort.");
            }

            // Add the candidate to the cohort
            cohort.CandidatesList.Add(candidate);

            // Set the cohort ID on the candidate entity
            candidate.CohortId = cohortId;

            // Update the cohort and save changes
            _context.Cohorts.Update(cohort);
            await _context.SaveChangesAsync();
        }
    }
}