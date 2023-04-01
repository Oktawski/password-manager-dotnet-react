using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Responses;
using PasswordManager.Services.Authentication;

namespace PasswordManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase 
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate(UserAuthenticateDto authenticateDto)
    {
        var response = await _service.Authenticate(authenticateDto);

        if (response.IsSuccess) 
            return Ok(response);

        return BadRequest(response);
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(UserCreateDto createDto)
    {
        var response = await _service.Register(createDto);
        
        if (response.IsSuccess())
            return Ok(response);
        
        return BadRequest(response);
    }
}   
