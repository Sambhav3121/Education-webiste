using Education.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(u => u.Courses)  // <-- Important: specify inverse navigation
                .HasForeignKey(c => c.TeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
