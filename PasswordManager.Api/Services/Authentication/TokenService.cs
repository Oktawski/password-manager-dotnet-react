using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Configuration;
using PasswordManager.Models;

namespace PasswordManager.Services.Authentication;

public class TokenService
{
    private readonly SecretOptions _secretOptions;

    public TokenService(
        IOptions<SecretOptions> options)
    {
        _secretOptions = options.Value;
    }

    public string CreateTokenForUserWithRolesAsync(ApplicationUser user, IList<string> roles)
    {
        var claims = CreateClaimsForUserWithRoles(user, roles);

        var token = CreateTokenForClaims(claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> CreateClaimsForUserWithRoles(ApplicationUser user, IList<string> roles)
    {
        var userClaims = CreateClaimsFromUser(user);
        
        userClaims.AddRange(CreateClaimsFromRoles(roles));

        return userClaims;
    }

    private List<Claim> CreateClaimsFromUser(ApplicationUser user) => new ()
    {
        new Claim(ClaimTypes.Name, user.UserName!),  
        new Claim("id", user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

    private List<Claim> CreateClaimsFromRoles(IList<string> roles) =>
        roles.Select(e => new Claim(ClaimTypes.Role, e)).ToList();

    private JwtSecurityToken CreateTokenForClaims(List<Claim> claims) => new (
        expires: DateTime.Now.AddHours(3),  
        claims: claims,  
        signingCredentials: new SigningCredentials(_secretOptions.SigningKey, _secretOptions.SecurityAlgorithm)
    );
}
