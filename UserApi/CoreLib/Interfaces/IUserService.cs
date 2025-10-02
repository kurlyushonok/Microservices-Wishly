using CoreLib.DTO;

namespace CoreLib.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Регистрация, создание нового пользователя
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    Task<UserResponseDto> RegisterAsync(UserRegisterDto registerDto);
    
    /// <summary>
    /// Аутентификация существующего пользователя
    /// </summary>
    /// <param name="authDto"></param>
    /// <returns></returns>
    Task<UserResponseDto> AuthenticateAsync(UserAuthDto authDto);
    
    /// <summary>
    /// Получение информации о пользователе по его имени
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<UserResponseDto?> GetByUsernameAsync(string username);
    
    /// <summary>
    /// Получение информации о пользователе по его id
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<UserResponseDto?> GetByIdAsync(string username);
    
    /// <summary>
    /// Обновление информации о пользователе
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    Task<UserResponseDto?> UpdateAsync(Guid id, UserUpdateDto updateDto);
    
    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Проверка существования пользователя с данным именем
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<bool> UsernameExistsAsync(string username);
}