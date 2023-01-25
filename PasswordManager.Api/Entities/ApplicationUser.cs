using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Password> Passwords { get; set; } = new List<Password>();


    [JsonIgnore]
    public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }

    [JsonIgnore]
    public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

    [JsonIgnore]
    public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
}
