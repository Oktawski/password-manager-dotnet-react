using System.Collections;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Entities;

namespace PasswordManager.Authorization
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Password> Passwords { get; set; } = new List<Password>();
    }
}