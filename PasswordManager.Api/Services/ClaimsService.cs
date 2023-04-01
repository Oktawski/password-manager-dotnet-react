using System.Security.Claims;
namespace PasswordManager.Services;

public abstract class ClaimsService
{
    private readonly ClaimsPrincipal _claims;

    public ClaimsService(ClaimsPrincipal claims)
    {
        _claims = claims;
    }

    protected string UserId => _claims.Claims.First(e => e.Type == "id").Value;
}
