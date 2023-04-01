using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Password> Passwords { get; set; } = new List<Password>();
}
