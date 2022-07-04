namespace PasswordManager.Requests;

public record AddPasswordRequest(
    string application,
    string value
);

public record EditPasswordRequest(
    string id,
    string application,
    string value
);