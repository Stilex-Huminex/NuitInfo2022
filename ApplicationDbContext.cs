using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuitInfo2022.Models.Entities;

namespace NuitInfo2022
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserMessageModel> UserMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");

            modelBuilder.Entity<UserMessageModel>().ToTable("UserMessages")
                .HasOne(um => um.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(um => um.UserId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
