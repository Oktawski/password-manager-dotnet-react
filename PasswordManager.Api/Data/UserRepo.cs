using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Models;
using PasswordManager.Responses;

namespace PasswordManager.Data;

public interface IUserRepo
{
    Task<AuthenticateResponse> Authenticate(UserAuthenticateDto authenticateDto, IConfiguration configuration);
    Task<RegisterResponse> Register(UserCreateDto createDto);
}

public class UserRepo : IUserRepo
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserRepo(UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<AuthenticateResponse> Authenticate(UserAuthenticateDto authenticateDto, IConfiguration configuration)
    {
        var user = await _userManager.FindByNameAsync(authenticateDto.Username);  
        if (user is null) 
            return new AuthenticateResponse("User does not exist", null);

        if (!VerifyPassword(authenticateDto.Password, user.PasswordHash)) 
            return new AuthenticateResponse("Wrong password", null);

        var userRoles = await _userManager.GetRolesAsync(user);  
        var authClaims = CreateClaimsForUser(user);
        authClaims.AddRange(CreateClaimsFromUserRoles(userRoles));
  
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Secret")!));  
  
        var token = CreateTokenForClaims(authClaims, authSigningKey);

        var signedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthenticateResponse($"Hello {user.UserName}", signedToken);
    }

    private bool VerifyPassword(string password, string actualPasswordHash) => BCrypt.Net.BCrypt.Verify(password, actualPasswordHash);

    private List<Claim> CreateClaimsForUser(ApplicationUser user) => new ()
    {
        new Claim(ClaimTypes.Name, user.UserName!),  
        new Claim("id", user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

    private List<Claim> CreateClaimsFromUserRoles(IList<string> roles) =>
        roles.Select(e => new Claim(ClaimTypes.Role, e)).ToList();

    private JwtSecurityToken CreateTokenForClaims(List<Claim> claims, SymmetricSecurityKey key) => new (
        expires: DateTime.Now.AddHours(3),  
        claims: claims,  
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );


    public async Task<RegisterResponse> Register(UserCreateDto createDto)
    {
        var userExists = await _userManager.FindByNameAsync(createDto.Username);

        if (userExists != null)
            return new RegisterResponse("User already exists", null);

        if (!ArePasswordsMatching(createDto.Password, createDto.PasswordConfirm))
            return new RegisterResponse("Paswords do not match", null);
        
        var user = _mapper.Map<UserCreateDto, ApplicationUser>(createDto);

        var result = await _userManager.CreateAsync(user);

        return result.Succeeded
            ? new RegisterResponse("User created", user)
            : new RegisterResponse("Something went wrong", null);
    }

    private bool ArePasswordsMatching(string password, string confirmPassword) =>
        password == confirmPassword;


}
