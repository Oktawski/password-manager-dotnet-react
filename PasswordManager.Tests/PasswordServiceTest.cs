using Moq;
using PasswordManager.Entities;
using System.Security.Claims;
using PasswordManager.Authorization;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Services;

namespace PasswordManager.Tests;

public class PasswordServiceTest : IDisposable
{
    private readonly PasswordService _service;
    private ClaimsPrincipal claimsPrincipal;
    private ApplicationDbContext db;

    public PasswordServiceTest()
    {
        claimsPrincipal = GetClaimsPrincipal();
        db = GetInMemoryDbContext();

        _service = new PasswordService(db, claimsPrincipal);
    }
    
    public void Dispose()
    {
    }

    private static ClaimsPrincipal GetClaimsPrincipal()
    {
        var claims = new List<Claim>()
        {
            new Claim("Id", "applicationUserId")
        };

        var identity = new ClaimsIdentity(claims);
        
        return new ClaimsPrincipal(identity);
    }

    private static ApplicationDbContext GetInMemoryDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase("TestDb");
         
        return new ApplicationDbContext(optionsBuilder.Options);
    }
    

    [Fact]
    public async void Add_Password()
    {
        var isSuccess = await _service.Add(new Password{ Value="Test" });

        Assert.Equal(true, isSuccess);

        var passwords = db.Passwords.ToList();
        Assert.Single(passwords);
    }
}