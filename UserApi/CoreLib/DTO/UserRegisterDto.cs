namespace CoreLib.DTO;

public class UserRegisterDto
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public required string Password { get; set; }
    
    /// <summary>
    /// Повторно введенный пароль пользователя
    /// </summary>
    public required string ConfirmPassword { get; set; }
}