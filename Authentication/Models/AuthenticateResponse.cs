namespace Authentication.Models;

public class AuthenticateResponse(User user, string token)
{
    public Guid Id { get; set; } = user.UserId;
    public string? FirstName { get; set; } = user.FirstName;
    public string? LastName { get; set; } = user.LastName;
    public string? Username { get; set; } = user.Username;
    public string? Token { get; set; } = token;
}
