namespace Domain.Dto;

public class WishlistResponseDto
{
    /// <summary>
    /// id вишлиста
    /// </summary>
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Название вишлиста
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание вишлиста
    /// </summary>
    public required string Description { get; set; }
}