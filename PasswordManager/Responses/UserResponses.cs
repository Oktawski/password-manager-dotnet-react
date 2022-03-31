using PasswordManager.Entities;

namespace PasswordManager.Responses
{
    public record AuthenticateResponse(string accessToken, string refreshToken);

    public class RegisterResponse
    {
        public string Message { get; set; }
        public User? User { get; set; }

        public RegisterResponse(string message, User? user)
        {
            Message = message;
            User = user;
        }

        public bool IsSuccess() => User != null;
    }
    
}