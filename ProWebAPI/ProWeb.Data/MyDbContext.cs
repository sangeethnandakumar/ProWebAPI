using Microsoft.EntityFrameworkCore;
using ProWeb.Entities;

namespace ProWeb.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Goal> Goals { get; set; }

        public MyDbContext()
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuration if any
            //modelBuilder.Entity<User>().HasOne(a => a.CreatedBy).WithMany().OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<User>().HasOne(a => a.UpdatedBy).WithMany().OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<Project>().HasOne(a => a.CreatedBy).WithMany().OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<Project>().HasOne(a => a.UpdatedBy).WithMany().OnDelete(DeleteBehavior.SetNull);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=DESKTOP-QJ02OLT\SQLEXPRESS;Database=Activity;Trusted_Connection=True;");
        }
    }
}