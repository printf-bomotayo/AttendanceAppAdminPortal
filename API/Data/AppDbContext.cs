using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
         public DbSet<Cohort> Cohorts { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole{Name = "Member", NormalizedName = "MEMBER"},
                new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"}
            );

            modelBuilder.Entity<Cohort>()
                .HasOne(c => c.TrainingProgram)
                .WithMany(tp => tp.CohortList)
                .HasForeignKey(c => c.TrainingProgramId);

            modelBuilder.Entity<AttendanceRecord>()
            .HasOne(ar => ar.Candidate)
            .WithMany(c => c.AttendanceRecords)
            .HasForeignKey(ar => ar.CandidateId);

           base.OnModelCreating(modelBuilder);
        }

    }
}