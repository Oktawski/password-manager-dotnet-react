using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Data;
using PasswordManager.Responses;

namespace PasswordManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase 
{
    private readonly IUserRepo _userRepo;

    public UserController(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpGet]
    public ActionResult<string?> UserName() => User.Identity?.Name;

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate(UserAuthenticateDto authenticateDto, IConfiguration configuration)
    {
        var response = await _userRepo.Authenticate(authenticateDto, configuration);

        if (response.IsSuccess) 
            return Ok(response);

        return BadRequest(response);
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(UserCreateDto createDto)
    {
        var response = await _userRepo.Register(createDto);
        
        if (response.IsSuccess())
            return Ok(response);
        
        return BadRequest(response);
    }
}   
