using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.Entities
{
    public class Password
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
        
        [ForeignKey("userId")]
        public string? UserId { get; init; }
        public ApplicationUser User { get; set; } = null!;
    }
}