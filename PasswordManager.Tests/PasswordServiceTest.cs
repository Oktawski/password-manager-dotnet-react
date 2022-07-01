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

        PopulateDb();
    }
    
    public void Dispose()
    {
        var users = db.Users;
        db.Users.RemoveRange(users);
        db.SaveChanges();

        var passwords = db.Passwords;
        db.Passwords.RemoveRange(passwords);
        db.SaveChanges();
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

    private void PopulateDb()
    {
        var user = new ApplicationUser()
        {
            UserName = "test", 
            Id = "applicationUser", 
            PasswordHash = "hashedPassword" 
        };

        db.Users.Add(user);
        db.SaveChanges();
    }
    

    [Fact]
    public void Get_Prepopulated_Users_Set()
    {
        var users = db.Users.ToList();

        Assert.NotEmpty(users);
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
    public async void Get_All_Returns_Empty_Collection()
    {
        var passwords = await _service.GetAll();

        Assert.Empty(passwords);
    }

    [Fact]
    public async void Add_Get_Password()
    {
        var passwordApplication = "application";
        var passwordValue = "value";

        var isSuccess = await _service.Add(new AddPasswordRequest(passwordApplication, passwordValue));

        Assert.True(isSuccess);

        var password = db.Passwords.ToList().FirstOrDefault();

        Assert.NotNull(password);
        Assert.Equal(passwordApplication, password!.Application);
        Assert.Equal(passwordValue, password!.Value);
    }
}