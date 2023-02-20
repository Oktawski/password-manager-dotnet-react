using System.ComponentModel.DataAnnotations;

public class UserAuthenticateDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserCreateDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string PasswordConfirm { get; set; } = string.Empty;
}
