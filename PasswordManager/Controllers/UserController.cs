using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Requests;
using PasswordManager.Responses;
using PasswordManager.Services;

namespace PasswordManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase 
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<string?> Test()
        {
            return User.Identity?.Name;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = await _userService.Authenticate(request);

            if (response.IsSuccess) 
                return Ok(response);

            return BadRequest(response);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            var response = await _userService.Register(request);
            
            if (response.IsSuccess())
                return Ok(response);
            
            return BadRequest(response);
        }
    }   
}