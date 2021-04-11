using Microsoft.EntityFrameworkCore;

namespace SimpleTask.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
         
        public DbSet<UserModel> Users { get; set; }
 
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasMany(um => um.Tasks)
                .WithOne(tm => tm.AssignedUser)
                .HasForeignKey(tm => tm.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}