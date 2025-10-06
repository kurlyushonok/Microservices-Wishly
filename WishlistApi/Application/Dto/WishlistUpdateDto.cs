namespace Application.Dto;

public class WishlistUpdateDto
{
    /// <summary>
    /// Идентификационный номер вишлиста
    /// </summary>
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Название вишлиста
    /// </summary>
    public string? NewTitle { get; set; }
    
    /// <summary>
    /// Описание вишлиста
    /// </summary>
    public string? NewDescription { get; set; }
}