namespace Authentication.Models.Requests;

public class AddUserRequest
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }

    public required string Password { get; set; }
    public required string EncryptedPassword { get; set; }
}
