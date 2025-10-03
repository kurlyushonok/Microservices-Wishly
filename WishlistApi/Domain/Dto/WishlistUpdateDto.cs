namespace Domain.Dto;

public class WishlistUpdateDto
{
    /// <summary>
    /// Название вишлиста
    /// </summary>
    public required string NewTitle { get; set; }
    
    /// <summary>
    /// Описание вишлиста
    /// </summary>
    public required string NewDescription { get; set; }
}