namespace PasswordManager.Requests
{
    public record AuthenticateRequest(string username, string password);

    public record RegisterRequest(
        string username,
        string email,
        string password,
        string passwordConfirm
    );
}