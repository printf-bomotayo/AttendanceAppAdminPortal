using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs.AttendanceDTOs;
using API.DTOs.CohortDTOs;
using API.DTOs.TrainingProgramDTOs;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services.TrainingProgramService

{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrainingProgramService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TrainingProgram> CreateTrainingProgramAsync(TrainingProgramDto trainingProgramDto)
        {
            // Create a new TrainingProgram entity from the DTO
            var trainingProgram = _mapper.Map<TrainingProgram>(trainingProgramDto);

            // Manually set the additional properties
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            trainingProgram.CreatedBy = currentUserId;
            trainingProgram.ModifiedBy = currentUserId;
            trainingProgram.CreatedDate = DateTime.UtcNow;
            trainingProgram.ModifiedDate = DateTime.UtcNow;

            // Save the training program to the database
            _context.TrainingPrograms.Add(trainingProgram);
            await _context.SaveChangesAsync();

            return trainingProgram;
        }



        public async Task<TrainingProgram> UpdateAsync(int trainingProgramId, TrainingProgramUpdateDto trainingProgramUpdateDto)
        {
            // Find the existing training program by ID
            var trainingProgram = await _context.TrainingPrograms.FindAsync(trainingProgramId);

            if (trainingProgram == null)
            {
                throw new KeyNotFoundException($"Training program with ID {trainingProgramId} not found.");
            }

            // Update the training program with the values from the DTO
            _mapper.Map(trainingProgramUpdateDto, trainingProgram);

            // Update the ModifiedBy and ModifiedDate fields
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            trainingProgram.ModifiedBy = currentUserId;
            trainingProgram.ModifiedDate = DateTime.UtcNow;

            // Save changes to the database
            _context.TrainingPrograms.Update(trainingProgram);
            await _context.SaveChangesAsync();

            return trainingProgram;
        }





        public async Task<TrainingProgramDto> GetByIdAsync(int id)
        {
            var trainingProgram = await _context.TrainingPrograms.Include(tp => tp.CohortList).FirstOrDefaultAsync(tp => tp.Id == id);

            var trainingProgramDto = new TrainingProgramDto()
            {
                Name = trainingProgram.Name,
                Description = trainingProgram.Description,
                Duration = trainingProgram.Duration,
                FacilitatingInstitution = trainingProgram.FacilitatingInstitution,
                StartDate = trainingProgram.StartDate,
                EndDate = trainingProgram.EndDate,
                Location = trainingProgram.Location,
                DurationUnit = trainingProgram.DurationUnit
            };

            return trainingProgramDto;
        }

        public async Task<List<TrainingProgramDto>> GetAllAsync()
        {
            var trainingPrograms = await _context.TrainingPrograms.Include(tp => tp.CohortList).ToListAsync();

            var trainingProgramDtos = trainingPrograms.Select(tp => new TrainingProgramDto
            {
                Name = tp.Name,
                Description = tp.Description,
                Duration = tp.Duration,
                FacilitatingInstitution = tp.FacilitatingInstitution,
                StartDate = tp.StartDate,
                EndDate = tp.EndDate,
                Location = tp.Location,
                DurationUnit = tp.DurationUnit
            }).ToList();

            return trainingProgramDtos;

        }


        public async Task AddAsync(TrainingProgramDto trainingProgramDto)
        {
            var trainingProgram = new TrainingProgram()
            {
                Name = trainingProgramDto.Name,
                Description = trainingProgramDto.Description,
                Duration = trainingProgramDto.Duration,
                FacilitatingInstitution = trainingProgramDto.FacilitatingInstitution,
                Location = trainingProgramDto.Location,
                StartDate = trainingProgramDto.StartDate,
                EndDate = trainingProgramDto.EndDate,
                DurationUnit = trainingProgramDto.DurationUnit

            };
            await _context.TrainingPrograms.AddAsync(trainingProgram);
            await _context.SaveChangesAsync();
        }



    
        public async Task DeleteAsync(int trainingProgramId)
        {
            var trainingProgram = await _context.TrainingPrograms.Include(tp => tp.CohortList).FirstOrDefaultAsync(tp => tp.Id == trainingProgramId);

            if (trainingProgram != null)
            {
                _context.TrainingPrograms.Remove(trainingProgram);
                await _context.SaveChangesAsync();
            }
        }


        // Method to add a cohort to a training program
        public async Task AddCohortToTrainingProgramAsync(int trainingProgramId, CohortCreateDto cohortCreateDto)
        {
            // Check if the training program exists
            var trainingProgram = await _context.TrainingPrograms
                .FirstOrDefaultAsync(tp => tp.Id == trainingProgramId);

            if (trainingProgram == null)
            {
                throw new KeyNotFoundException($"Training program with ID {trainingProgramId} not found.");
            }

            // Update the ModifiedBy and ModifiedDate fields
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Map the DTO to the Cohort entity
            var cohort = _mapper.Map<Cohort>(cohortCreateDto);

            // Set the TrainingProgramId on the cohort
            cohort.TrainingProgramId = trainingProgramId;

            // Set administrative details
            cohort.CreatedDate = DateTime.UtcNow;
            cohort.ModifiedDate = DateTime.UtcNow;
            cohort.CreatedBy = currentUserId; 
            cohort.ModifiedBy = currentUserId; 

            // Add the cohort to the database
            await _context.Cohorts.AddAsync(cohort);
            await _context.SaveChangesAsync();
        }


        //public async Task<TrainingProgram> GetTrainingProgramByIdAsync(int id)
        //{
        //    return await _context.TrainingPrograms
        //                        .Include(tp => tp.CohortList)
        //                        .FirstOrDefaultAsync(tp => tp.Id == id);
        //}


    }
}