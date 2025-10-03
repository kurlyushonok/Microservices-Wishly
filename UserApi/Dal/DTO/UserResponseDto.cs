namespace Dal.DTO;

public class UserResponseDto
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string Username { get; set; }
}