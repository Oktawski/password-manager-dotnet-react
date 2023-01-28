using System.Text.Json.Serialization;
using PasswordManager.Models;

namespace PasswordManager.Responses;

public class AuthenticateResponse
{
    public string Message { get; set; } = "";
    public string? AccessToken { get; set; }

    public AuthenticateResponse(string message, string? token)
    {
        Message = message;
        AccessToken = token;
    }

    [JsonIgnore]
    public bool IsSuccess => AccessToken != null;
}

public class RegisterResponse
{
    public string Message { get; set; }
    public UserResponse? User { get; set; }

    public RegisterResponse(string message, ApplicationUser? user)
    {
        Message = message;
        User = user is null 
            ? null 
            : new UserResponse(user.UserName!, user.Email!);
    }

    public bool IsSuccess() => User != null;
}

public record UserResponse(
    string username,
    string email
);
