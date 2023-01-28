namespace PasswordManager.Dtos;

public class PasswordReadDto
{
    public Guid Id { get; set; }

    public string Application { get; set; } = string.Empty; 

    public string Login { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}

public class PasswordCreateDto
{
    public string Application { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class PasswordEditDto : PasswordReadDto 
{ }
