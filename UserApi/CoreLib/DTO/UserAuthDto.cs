namespace CoreLib.DTO;

public class UserAuthDto
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public required string Password { get; set; }
}