using System.Security.Claims;
using AutoMapper;
using PasswordManager.Dtos;
using PasswordManager.Models;
using PasswordManager.Profiles;
using PasswordManager.Services;
using PasswordManager.Tests.Configuration;

namespace PasswordManager.Tests;

public class PasswordServiceTest : IDisposable
{
    private readonly PasswordService _service;
    private ClaimsPrincipal claimsPrincipal;
    private ApplicationDbContext context;

    public PasswordServiceTest()
    {
        claimsPrincipal = TestsConfiguration.GetClaimsPrincipal();
        context = TestsConfiguration.GetInMemoryDbContext();

        var passwordsProfile = new PasswordsProfile();
        var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(passwordsProfile));
        var mapper = new Mapper(mapperConfiguration);

        _service = new PasswordService(context, claimsPrincipal, mapper);

        PopulateDb();
    }
    
    public void Dispose()
    {
        var users = context.Users;
        context.Users.RemoveRange(users);
        context.SaveChanges();

        var passwords = context.Passwords;
        context.Passwords.RemoveRange(passwords);
        context.SaveChanges();
    }
    
    private void PopulateDb()
    {
        var user = new ApplicationUser()
        {
            UserName = "test", 
            Id = "applicationUserId", 
            PasswordHash = "hashedPassword" 
        };

        context.Users.Add(user);
        context.SaveChanges();
    }

    [Fact]
    public void GetPrepopulatedUsers_ShouldNotBeEmpty()
    {
        var users = context.Users.ToList();

        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task AddPassword_ShouldReturnTrue()
    {
        var passwordCreateDto = new PasswordCreateDto
        { 
            Application = "application", 
            Login = "login", 
            Value = "value" 
        };

        var isSuccess = await _service.AddAsync(passwordCreateDto);

        Assert.True(isSuccess);

        var passwords = context.Passwords.ToList();

        Assert.NotEmpty(passwords);
    }

    [Fact]
    public async Task GetAllPasswords_ShouldReturnEmptyCollection()
    {
        var passwords = await _service.GetAllAsync();

        Assert.Empty(passwords);
    }

    [Fact]
    public async Task AddPassword_ThenGetAddedPassword_PropertiesShouldBeEqual()
    {
        var passwordApplication = "application";
        var passwordLogin = "login";
        var passwordValue = "value";

        var passwordCreateDto = new PasswordCreateDto
        {
            Application = passwordApplication,
            Login = passwordLogin,
            Value = passwordValue
        };

        var isSuccess = await _service.AddAsync(passwordCreateDto);

        Assert.True(isSuccess);

        var password = context.Passwords.ToList().FirstOrDefault();

        Assert.NotNull(password);
        Assert.IsType<Guid>(password!.Id);
        Assert.Equal(passwordApplication, password!.Application);
        Assert.Equal(passwordLogin, password!.Login);
        Assert.Equal(passwordApplication.ToUpper(), password!.ApplicationNormalized);
        Assert.Equal(passwordValue, password!.Value);
        Assert.Equal(Configuration.TestsConfiguration.USER_ID, password!.UserId);
    }

    [Fact]
    public async Task AddPassword_ThenGetAddedPasswordById_ShouldReturnAddedPassword()
    {
        var passwordApplication = "application";
        var passwordLogin = "login";
        var passwordValue = "value";

        var passwordCreateDto = new PasswordCreateDto
        {
            Application = passwordApplication,
            Login = passwordLogin,
            Value = passwordValue
        };

        var isSuccess = await _service.AddAsync(passwordCreateDto);

        Assert.True(isSuccess);

        var addedPassword = (await _service.GetAllAsync()).FirstOrDefault();
        Assert.NotNull(addedPassword);

        var addedPasswordId = addedPassword!.Id;

        var passwordById = await _service.GetByIdAsync(addedPasswordId);
        Assert.NotNull(passwordById);
        Assert.Equal(addedPassword.Application, passwordById!.ApplicationNormalized);
        Assert.Equal(addedPassword.Login, passwordById!.Login);
    }

    [Fact]
    public async Task AddPassword_ThenGetAddedPassword_ThenEditPassword_ShouldReturnTrue()
    {
        var passwordApplication = "appBeforeUpdate";
        var passwordLogin = "loginBeforeUpdate";
        var passwordValue = "valueBeforeUpdate";

        var passwordCreateDto = new PasswordCreateDto
        {
            Application = passwordApplication,
            Login = passwordLogin,
            Value = passwordValue
        };

        var isAdded = await _service.AddAsync(passwordCreateDto);
        Assert.True(isAdded);

        var addedPassword = (await _service.GetAllAsync()).First();

        var editPasswordApplication = "applicationAfterUpdate";
        var editPasswordLogin = "loginAfterUpdate";
        var editPasswordValue = "valueAfterUpdate";

        var editPasswordRequest = new PasswordEditDto 
        {
            Id = addedPassword.Id,
            Application = editPasswordApplication,
            Login = editPasswordLogin,
            Value = editPasswordValue
        };

        var isUpdated = await _service.EditByIdAsync(editPasswordRequest);
        Assert.True(isUpdated);
    }
}
