using CoreLib.Entities;

namespace Dal.Entities;

public class User : BaseEntityDal<Guid>
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// Хеш-код пароля пользователя
    /// </summary>
    public required string PasswordHash { get; set; }
}