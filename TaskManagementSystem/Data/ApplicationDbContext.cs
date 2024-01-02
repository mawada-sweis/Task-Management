using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;


namespace TaskManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TaskModel>? Todos { get; set; }
        public DbSet<ApplicationUser>? User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId);
        }
    }
}
