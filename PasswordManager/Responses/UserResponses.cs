using PasswordManager.Authorization;
using PasswordManager.Entities;

namespace PasswordManager.Responses
{
    public class AuthenticateResponse
    {
        public string Message { get; set; } = "";
        public string? AccessToken { get; set; }

        public AuthenticateResponse(string message, string? token)
        {
            Message = message;
            AccessToken = token;
        }

        public bool IsSuccess => AccessToken != null;
    }

    public class RegisterResponse
    {
        public string Message { get; set; }
        public ApplicationUser? User { get; set; }

        public RegisterResponse(string message, ApplicationUser? user)
        {
            Message = message;
            User = user;
        }

        public bool IsSuccess() => User != null;
    }
    
}