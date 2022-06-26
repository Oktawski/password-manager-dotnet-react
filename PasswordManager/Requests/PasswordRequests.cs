namespace PasswordManager.Requests;

public record AddPasswordRequest(
    string application,
    string value
);