using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PasswordManager.Configuration;

public class SecretOptions
{
    public string Value { get; set; } = string.Empty;


    public SymmetricSecurityKey SigningKey => new (Encoding.UTF8.GetBytes(Value));
    public string SecurityAlgorithm => SecurityAlgorithms.HmacSha256; 
}
