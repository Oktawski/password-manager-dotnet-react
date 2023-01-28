using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PasswordManager.Models;

public class Password
{
    [Key]
    public Guid Id { get; set; }

    private string _application { get; set; } = string.Empty;
    
    public string ApplicationNormalized { get => _application.ToUpper(); }
    
    [Required]
    [JsonIgnore]
    public string Application { get => _application; set => _application = value; }

    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string Value { get; set; } = string.Empty;
    
    [ForeignKey("userId")]
    [JsonIgnore]
    public string? UserId { get; set; }
    
    [JsonIgnore]
    public ApplicationUser User { get; set; } = null!;
}
