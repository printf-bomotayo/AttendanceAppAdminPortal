using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        // public AppDbContext(DbContextOptions options) : base(options)
        // {
        // }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
         public DbSet<Cohort> Cohorts { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CandidatesGroup> Groups { get; set; }

        

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
                .HasForeignKey(c => c.TrainingProgramId)
                .OnDelete(DeleteBehavior.Cascade);

             modelBuilder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.Candidate)
                .WithMany(c => c.AttendanceRecords)
                .HasForeignKey(ar => ar.CandidateId)
                .OnDelete(DeleteBehavior.Cascade); // This will delete attendance records when a candidate is deleted

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Candidate)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.CandidateId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Group)
                .WithMany()
                .HasForeignKey(n => n.GroupId);

            modelBuilder.Entity<CandidatesGroup>()
                .HasMany(g => g.Candidates);
                //.WithMany(u => u.Groups);

            base.OnModelCreating(modelBuilder);
        }

    }
}