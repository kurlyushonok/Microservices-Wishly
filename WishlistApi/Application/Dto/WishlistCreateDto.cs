namespace Application.Dto;

public class WishlistCreateDto
{
    /// <summary>
    /// Название вишлиста
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание вишлиста
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Дата создания вишлиста
    /// </summary>
    public required DateTime CreatedAt { get; set; }
}