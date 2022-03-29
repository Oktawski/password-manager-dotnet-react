using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public virtual List<Password> Passwords { get; set; } = new List<Password>();
    }
}