using Application.Dto;

namespace Domain.Logic.Interfaces;

public interface IWishlistService
{
    /// <summary>
    /// Создание нового вишлиста
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    Task<WishlistResponseDto> CreateAsync(WishlistCreateDto createDto);

    /// <summary>
    /// Обновление существующего вишлиста
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    Task<WishlistResponseDto> UpdateAsync(WishlistUpdateDto updateDto);

    /// <summary>
    /// Удаление существующего вишлиста
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    
    //TODO: точно ли реализовывать через массив?
    /// <summary>
    /// Получение всех вишлистов пользователя по его id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<WishlistResponseDto>> GetAllAsync(Guid userId);
    
    /// <summary>
    /// Получение информации о вишлисте по его id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<WishlistResponseDto> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Получение информации о вишлисте по его названию
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    Task<WishlistResponseDto> GetByTitleAsync(string title);

    /// <summary>
    /// Проверка существования вишлиста с данным названием у данного пользователя
    /// </summary>
    /// <param name="title"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> ExistsWithTitleAsync(string title, Guid id);
}