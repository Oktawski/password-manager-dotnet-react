
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Requests;
using PasswordManager.Responses;
using PasswordManager.Services;

namespace PasswordManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase 
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult<string> Test()
        {
            return new OkObjectResult("It works");
        }


        [HttpPost("authenticate")]
        public ActionResult<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest request)
        {
            var authenticationResponse = _userService.Authenticate(request);

            return new BadRequestResult();
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            var registerResponse = await _userService.Register(request);

            if (!registerResponse.IsSuccess()) return BadRequest(registerResponse);
            
            return Ok(registerResponse);
        }
    }
}