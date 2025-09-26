namespace CoreLib.Entities;

public class UserResponseDTO
{
    public required Guid Id { get; init; }
    public required string Username { get; set; }
}