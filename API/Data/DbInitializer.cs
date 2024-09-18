using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class DbInitializer
{
    public static async Task Initialize(AppDbContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new User 
            {
                UserName = "Victor",
                Email = "victor@test.com"
            };

            await userManager.CreateAsync(user, "P@$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");

            var admin = new User
            {
                UserName = "admin",
                Email = "admin@test.com"
            };

            await userManager.CreateAsync(admin, "P@$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Member", "Admin"});
        }

        if (!context.TrainingPrograms.Any())
        {
            var trainingPrograms = new List<TrainingProgram>
            {
                new TrainingProgram
                {
                    Title = "Software Engineering Bootcamp",
                    Description = "An intensive bootcamp for aspiring software engineers.",
                    FacilitatingInstitution = "Tech Academy",
                    Location = "New York, NY",
                    Duration = 12,
                    DurationUnit = UnitOfDuration.Week,
                    CohortList = new List<Cohort>()
                },
                new TrainingProgram
                {
                    Title = "Data Science Program",
                    Description = "A comprehensive program on data science and analytics.",
                    FacilitatingInstitution = "Data Institute",
                    Location = "San Francisco, CA",
                    Duration = 6,
                    DurationUnit = UnitOfDuration.Month,
                    CohortList = new List<Cohort>()
                },
                new TrainingProgram
                {
                    Title = "Cybersecurity Certification",
                    Description = "A certification program for cybersecurity professionals.",
                    FacilitatingInstitution = "Security Experts",
                    Location = "Austin, TX",
                    Duration = 1,
                    DurationUnit = UnitOfDuration.Year,
                    CohortList = new List<Cohort>()
                }
            };
            await context.TrainingPrograms.AddRangeAsync(trainingPrograms);
            await context.SaveChangesAsync();
        }

        if (!context.Cohorts.Any())
        {
            var trainingPrograms = await context.TrainingPrograms.ToListAsync();

            var cohorts = new List<Cohort>
            {
                new Cohort
                {
                    Name = "Cohort 1",
                    Description = "First Cohort Description",
                    StartDate = new DateTime(2023, 01, 01),
                    EndDate = new DateTime(2023, 06, 30),
                    Status = Cohort.ActivityStatus.Active,
                    CandidatesList = new List<Candidate>(),
                    TrainingProgramId = trainingPrograms.FirstOrDefault().Id,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin"
                },
                new Cohort
                {
                    Name = "Cohort 2",
                    Description = "Second Cohort Description",
                    StartDate = new DateTime(2023, 07, 01),
                    EndDate = new DateTime(2023, 12, 31),
                    Status = Cohort.ActivityStatus.Pending,
                    CandidatesList = new List<Candidate>(),
                    TrainingProgramId = trainingPrograms.ElementAt(1).Id,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin"
                },
                new Cohort
                {
                    Name = "Cohort 3",
                    Description = "Third Cohort Description",
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 06, 30),
                    Status = Cohort.ActivityStatus.Completed,
                    CandidatesList = new List<Candidate>(),
                    TrainingProgramId = trainingPrograms.ElementAt(2).Id,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin"
                }
            };
            await context.Cohorts.AddRangeAsync(cohorts);
            await context.SaveChangesAsync();
        }

        if (!context.Candidates.Any())
        {
            var cohorts = await context.Cohorts.ToListAsync();

            var candidates = new List<Candidate>
            {
                new Candidate 
                { 
                    FirstName = "John", 
                    LastName = "Doe", 
                    Email = "john.doe@example.com", 
                    Gender = Gender.Male, 
                    PhoneNumber = "123-456-7890", 
                    StaffId = "S001", 
                    Department = "Engineering", 
                    CohortId = cohorts.FirstOrDefault().Id
                },
                new Candidate 
                { 
                    FirstName = "Jane", 
                    LastName = "Doe", 
                    Email = "jane.doe@example.com", 
                    Gender = Gender.Female, 
                    PhoneNumber = "987-654-3210", 
                    StaffId = "S002", 
                    Department = "Marketing", 
                    CohortId = cohorts.ElementAt(1).Id
                }
            };

            await context.Candidates.AddRangeAsync(candidates);
            await context.SaveChangesAsync();
        }

        if (!context.AttendanceRecords.Any())
        {
            var candidates = await context.Candidates.ToListAsync();

            var attendanceRecords = new List<AttendanceRecord>
            {
                new AttendanceRecord 
                { 
                    CandidateId = candidates.First().Id, 
                    Date = DateTime.Now, 
                    Status = AttendanceStatus.Early, 
                    Location = "Office", 
                    Latitude = 40.7128, 
                    Longitude = -74.0060 
                },
                new AttendanceRecord 
                { 
                    CandidateId = candidates.ElementAt(1).Id, 
                    Date = DateTime.Now, 
                    Status = AttendanceStatus.Late, 
                    Location = "Remote",
                    Latitude = 34.0522, 
                    Longitude = -118.2437
                }
            };
            await context.AttendanceRecords.AddRangeAsync(attendanceRecords);
            await context.SaveChangesAsync();
        }
    }
}
}