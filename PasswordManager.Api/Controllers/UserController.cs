using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Data;
using PasswordManager.Requests;
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
    public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest request, IConfiguration configuration)
    {
        var response = await _userRepo.Authenticate(request, configuration);

        if (response.IsSuccess) 
            return Ok(response);

        return BadRequest(response);
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
    {
        var response = await _userRepo.Register(request);
        
        if (response.IsSuccess())
            return Ok(response);
        
        return BadRequest(response);
    }
}   
