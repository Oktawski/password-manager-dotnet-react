using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;

namespace PasswordManager.Data;
    
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Password> Passwords { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Password>()
            .HasOne(p => p.User)
            .WithMany(u => u.Passwords)
            .HasForeignKey(p => p.UserId);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Passwords)
            .WithOne(p => p.User)
            .OnDelete(DeleteBehavior.Cascade);

    }
}