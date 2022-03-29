
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Requests;
using PasswordManager.Responses;
using PasswordManager.Services;

namespace PasswordManager.Controllers
{
    [ApiController]
    public class UserController 
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var authenticationResponse = _userService.Authenticate(request);

            return new BadRequestResult();
        }


        [HttpPost]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            var registerResponse = _userService.Register(request);
            
            return new BadRequestResult();
        }
    }
}