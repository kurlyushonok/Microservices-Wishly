using Domain.Entities;

namespace Domain.Interfaces;

public interface IWishlistRepository
{
    /// <summary>
    /// Создание вишлиста
    /// </summary>
    /// <param name="wishlist"></param>
    /// <returns></returns>
    Task AddAsync(Wishlist wishlist);

    /// <summary>
    /// Обновление информации о вишлисте
    /// </summary>
    /// <param name="wishlist"></param>
    /// <returns></returns>
    Task UpdateAsync(Wishlist wishlist);

    /// <summary>
    /// Удаление вишлиста
    /// </summary>
    /// <param name="wishlist"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Получение всех вишлистов пользователя по его id
    /// </summary>
    /// <returns></returns>
    Task<List<Wishlist>?> GetAllByUserIdAsync(Guid userId);

    /// <summary>
    /// Получение вишлиста по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Wishlist?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Получение вишлиста по названию
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    Task<Wishlist?> GetByTitleAsync(string title);
    
    /// <summary>
    /// Проверка существования вишлиста с данным названием у данного пользователя
    /// </summary>
    /// <param name="title"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> ExistsWithTitleAsync(string title, Guid userId);
}