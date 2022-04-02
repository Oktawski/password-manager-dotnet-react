using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Authorization;
using PasswordManager.Requests;
using PasswordManager.Responses;

namespace PasswordManager.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
        Task<RegisterResponse> Register(RegisterRequest request);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _repository;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext repository, IConfiguration configuration)
        {
            _userManager = userManager;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.username);  
            var checkPasswords = BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash);

            if (user == null) 
                return new AuthenticateResponse("User does not exist", null);
            
            if (!checkPasswords) 
                return new AuthenticateResponse("Wrong password", null);

            var userRoles = await _userManager.GetRolesAsync(user);  
  
            var authClaims = new List<Claim>  
            {  
                new Claim(ClaimTypes.Name, user.UserName),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            };  
  
            foreach (var userRole in userRoles)  
            {  
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
            }  
  
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Secret")));  
  
            var token = new JwtSecurityToken(  
                expires: DateTime.Now.AddHours(3),  
                claims: authClaims,  
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
            );  
  
            return new AuthenticateResponse($"Hello {user.UserName}", new JwtSecurityTokenHandler().WriteToken(token));
        }


        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var userExists = await _userManager.FindByNameAsync(request.username);

            if (userExists != null)
                return new RegisterResponse("User already exists", null);

            if (!ArePasswordsMatching(request.password, request.passwordConfirm))
                return new RegisterResponse("Paswords do not match", null);
            
            var user = new ApplicationUser()
            {
                Email = request.email,
                UserName = request.username,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.password)
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
                return new RegisterResponse("User created", null);

            return new RegisterResponse("Something went wrong", null);
        }

        public bool ArePasswordsMatching(string password, string confirmPassword) =>
            password == confirmPassword;

    }
}