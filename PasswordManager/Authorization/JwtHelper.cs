using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Entities;

namespace PasswordManager.Authorization
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
        Guid? ValidateToken(string? token);
    }
    
    public class JwtHelper : IJwtHelper
    {
        private readonly string _secret;

        public JwtHelper(string secret)
        {
            _secret = secret;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new  JwtSecurityTokenHandler();
            var key = GetKey();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key), 
                                        SecurityAlgorithms.HmacSha256Signature
                                    )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateToken(string? token)
        {
            if (token is null) 
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var claimId = jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                if (claimId is null) return null;

                return new Guid(claimId);
            }
            catch
            {
                return null;
            }
        }

        private byte[] GetKey() => Encoding.ASCII.GetBytes(_secret);
    }
}

