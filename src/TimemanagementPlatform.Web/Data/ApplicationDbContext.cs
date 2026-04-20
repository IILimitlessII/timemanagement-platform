using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimemanagementPlatform.Web.Models;

namespace TimemanagementPlatform.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Semester> Semesters { get; set; } = default!;
        public DbSet<Module> Modules { get; set; } = default!;
        public DbSet<ModuleHour> ModuleHours { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Module>()
                .HasOne(m => m.User)
                .WithMany(u => u.Modules)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for the User relationship

            modelBuilder.Entity<Module>()
                .HasOne(m => m.Semester)
                .WithMany(s => s.Modules)
                .HasForeignKey(m => m.SemesterId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for the Semester relationship

            modelBuilder.Entity<ModuleHour>()
                .HasOne(mh => mh.User)
                .WithMany(u => u.ModuleHours)
                .HasForeignKey(mh => mh.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for the User relationship

            modelBuilder.Entity<ModuleHour>()
                .HasOne(mh => mh.Semester)
                .WithMany(s => s.ModuleHours)
                .HasForeignKey(mh => mh.SemesterId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for the Semester relationship
        }
    }
}