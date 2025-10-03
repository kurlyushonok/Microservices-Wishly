using Domain.Entities;

namespace Infrastucture.Interfaces;

public interface IWishlistRepository
{
    /// <summary>
    /// Добавление вишлиста
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
    Task DeleteAsync(Wishlist wishlist);

    /// <summary>
    /// Получение всех вишлистов пользователя по его id
    /// </summary>
    /// <returns></returns>
    Task<Wishlist[]> GetAllByUserId();

    /// <summary>
    /// Получение вишлиста по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Wishlist> GetById(Guid id);
    
    /// <summary>
    /// Получение вишлиста по названию
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    Task<Wishlist> GetByTitle(string title);
    
    /// <summary>
    /// Проверка существования вишлиста с данным названием у данного пользователя
    /// </summary>
    /// <param name="title"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> ExistsWithTitleAsync(string title, Guid userId);
}