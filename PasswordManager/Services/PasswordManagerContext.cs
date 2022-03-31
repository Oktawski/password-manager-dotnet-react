using Microsoft.EntityFrameworkCore;
using PasswordManager.Entities;

namespace PasswordManager.Services
{
    public class PasswordManagerContext : DbContext
    {
        public DbSet<Password> Passwords { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public PasswordManagerContext(DbContextOptions<PasswordManagerContext> options)
            : base(options)
        {}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Password>()
                .HasOne(p => p.User)
                .WithMany(u => u.Passwords)
                .HasForeignKey(p => p.UserId);
        }
    }
}