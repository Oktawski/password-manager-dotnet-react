using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Models;
using PasswordManager.Responses;

namespace PasswordManager.Services.Authentication;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(UserAuthenticateDto authenticateDto);
    Task<RegisterResponse> Register(UserCreateDto createDto);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthenticateResponse> Authenticate(UserAuthenticateDto authenticateDto)
    {
        var user = await _userManager.FindByNameAsync(authenticateDto.Username);  
        if (user is null) 
            return new AuthenticateResponse("User does not exist", null);

        if (!VerifyPassword(authenticateDto.Password, user.PasswordHash!)) 
            return new AuthenticateResponse("Wrong password", null);

        var roles = await _userManager.GetRolesAsync(user);

        var signedToken = _tokenService.CreateTokenForUserWithRolesAsync(user, roles);  
  
        return new AuthenticateResponse($"Hello {user.UserName}", signedToken);
    }

    private bool VerifyPassword(string password, string actualPasswordHash) => BCrypt.Net.BCrypt.Verify(password, actualPasswordHash);


    public async Task<RegisterResponse> Register(UserCreateDto createDto)
    {
        var user = await _userManager.FindByNameAsync(createDto.Username);

        if (user != null)
            return new RegisterResponse("User already exists", null);

        if (!ArePasswordsMatching(createDto.Password, createDto.PasswordConfirm))
            return new RegisterResponse("Paswords do not match", null);
        
        var userToCreate = _mapper.Map<UserCreateDto, ApplicationUser>(createDto);

        var result = await _userManager.CreateAsync(userToCreate);

        return result.Succeeded
            ? new RegisterResponse("User created", user)
            : new RegisterResponse("Something went wrong", null);
    }

    private bool ArePasswordsMatching(string password, string confirmPassword) =>
        password == confirmPassword;
}
