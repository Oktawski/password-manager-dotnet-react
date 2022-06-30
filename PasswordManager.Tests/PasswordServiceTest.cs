using Moq;
using PasswordManager.Entities;
using System.Security.Claims;
using PasswordManager.Authorization;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Services;
using PasswordManager.Requests;

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
            new Claim("id", "applicationUserId")
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
        var isSuccess = await _service.Add(new AddPasswordRequest("application", "value"));

        Assert.True(isSuccess);

        var passwords = db.Passwords.ToList();

        Assert.NotEmpty(passwords);
    }

    [Fact]
    public async void Get_All()
    {
        var passwords = await _service.GetAll();

        Assert.Empty(passwords);
    }
}