using System.Security.Claims;
using PasswordManager.Data;
using PasswordManager.Models;
using PasswordManager.Requests;
using PasswordManager.Tests.Configuration;

namespace PasswordManager.Tests;

public class PasswordServiceTest : IDisposable
{
    private readonly PasswordRepo _repo;
    private ClaimsPrincipal claimsPrincipal;
    private ApplicationDbContext db;

    public PasswordServiceTest()
    {
        claimsPrincipal = TestsConfiguration.GetClaimsPrincipal();
        db = TestsConfiguration.GetInMemoryDbContext();

        _repo = new PasswordRepo(db, claimsPrincipal);

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
    
    private void PopulateDb()
    {
        var user = new ApplicationUser()
        {
            UserName = "test", 
            Id = "applicationUserId", 
            PasswordHash = "hashedPassword" 
        };

        db.Users.Add(user);
        db.SaveChanges();
    }

    [Fact]
    public void GetPrepopulatedUsers_ShouldNotBeEmpty()
    {
        var users = db.Users.ToList();

        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task AddPassword_ShouldReturnTrue()
    {
        var isSuccess = await _repo.AddAsync(new AddPasswordRequest("application", "login", "value"));

        Assert.True(isSuccess);

        var passwords = db.Passwords.ToList();

        Assert.NotEmpty(passwords);
    }

    [Fact]
    public async Task GetAllPasswords_ShouldReturnEmptyCollection()
    {
        var passwords = await _repo.GetAllAsync();

        Assert.Empty(passwords);
    }

    [Fact]
    public async Task AddPassword_ThenGetAddedPassword_PropertiesShouldBeEqual()
    {
        var passwordApplication = "application";
        var passwordLogin = "login";
        var passwordValue = "value";

        var isSuccess = await _repo.AddAsync(new AddPasswordRequest(passwordApplication, passwordLogin, passwordValue));

        Assert.True(isSuccess);

        var password = db.Passwords.ToList().FirstOrDefault();

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

        var passwordToAdd = new AddPasswordRequest(passwordApplication, passwordLogin, passwordValue);

        var isSuccess = await _repo.AddAsync(passwordToAdd);

        Assert.True(isSuccess);

        var addedPassword = (await _repo.GetAllAsync()).FirstOrDefault();
        Assert.NotNull(addedPassword);

        var addedPasswordId = addedPassword!.Id;

        var passwordById = await _repo.GetByIdAsync(addedPasswordId.ToString());
        Assert.NotNull(passwordById);
        Assert.Equal(addedPassword.ApplicationNormalized, passwordById!.ApplicationNormalized);
        Assert.Equal(addedPassword.Login, passwordById!.Login);
    }

    [Fact]
    public async Task GetPasswordByWrongId_ShouldThrowFormatException_ThenGetPasswordByInexistingId_ShouldReturnNull()
    {
        await Assert.ThrowsAsync<System.FormatException>(async () => await _repo.GetByIdAsync("wrong guid format"));

        var notExistingId = new Guid().ToString();
        var inexistingPassword = await _repo.GetByIdAsync(notExistingId);

        Assert.Null(inexistingPassword);
    }

    [Fact]
    public async Task AddPassword_ThenGetAddedPassword_ThenEditPassword_ShouldReturnTrue()
    {
        var passwordApplication = "appBeforeUpdate";
        var passwordLogin = "loginBeforeUpdate";
        var passwordValue = "valueBeforeUpdate";

        var passwordToAdd = new AddPasswordRequest(passwordApplication, passwordLogin, passwordValue);

        var isAdded = await _repo.AddAsync(passwordToAdd);
        Assert.True(isAdded);

        var addedPassword = (await _repo.GetAllAsync()).First();

        var editPasswordApplication = "applicationAfterUpdate";
        var editPasswordLogin = "loginAfterUpdate";
        var editPasswordValue = "valueAfterUpdate";

        var editPasswordRequest = new EditPasswordRequest
        (
            addedPassword.Id.ToString(),
            editPasswordApplication,
            editPasswordLogin,
            editPasswordValue
        );

        var isUpdated = await _repo.EditByIdAsync(editPasswordRequest);
        Assert.True(isUpdated);
    }
}
