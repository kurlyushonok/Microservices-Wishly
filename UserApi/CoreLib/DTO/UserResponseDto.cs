namespace CoreLib.DTO;

public class UserResponseDto
{
    public required Guid Id { get; init; }
    public required string Username { get; set; }
}