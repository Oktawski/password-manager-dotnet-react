using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Services;

namespace PasswordManager.Tests.Configuration;

public static class TestsConfiguration
{
    public const string USER_ID = "applicationUserId";
    public const string ID_CLAIM = "id";

    public static ClaimsPrincipal GetClaimsPrincipal()
    {
        var claims = new List<Claim>()
        {
            new Claim(ID_CLAIM, USER_ID)
        };

        var identity = new ClaimsIdentity(claims);
        
        return new ClaimsPrincipal(identity);
    }

    public static ApplicationDbContext GetInMemoryDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase("TestDb");
         
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
