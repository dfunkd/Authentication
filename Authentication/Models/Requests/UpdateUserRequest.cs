﻿namespace Authentication.Models.Requests;

public class UpdateUserRequest
{
    public Guid UserId { get; set; }

    public DateTime? LastLogin { get; set; }

    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }

    public required string Password { get; set; }
    public required string EncryptedPassword { get; set; }
}
