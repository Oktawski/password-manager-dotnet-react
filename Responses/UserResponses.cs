namespace PasswordManager.Responses
{
    public record AuthenticateResponse(string accessToken, string refreshToken);

    public record RegisterResponse(Entities.User user);
}