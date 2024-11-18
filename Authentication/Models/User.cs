using System.Text.Json.Serialization;

namespace Authentication.Models;

public class User : BaseModel
{
    public Guid UserId { get; set; }

    public DateTime? LastLogin { get; set; }

    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    [JsonIgnore]
    public string EncryptedPassword { get; set; }
}
