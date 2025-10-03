using CoreLib.Entities;
using Dal.Entities;

namespace Dal.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Добавление пользователя в базу данных
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task AddAsync(User user);
    
    /// <summary>
    /// Обновление информации о пользователе в базе данных
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task UpdateAsync(User user);
    
    /// <summary>
    /// Удаление пользователя из базы данных
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Получение данных о пользователе по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Получение данных о пользователе по его имени
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<User?> GetByUsernameAsync(string username);
    
    /// <summary>
    /// Проверка существования пользователя в базе данных с данным именем
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<bool> ExistsUsernameAsync(string username);

}