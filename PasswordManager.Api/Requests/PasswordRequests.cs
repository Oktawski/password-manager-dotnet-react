namespace PasswordManager.Requests;

public record AddPasswordRequest(
    string application,
    string login,
    string value
);

public record EditPasswordRequest(
    string id,
    string application,
    string login,
    string value
);