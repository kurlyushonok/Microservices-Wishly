using CoreLib.Entities;

namespace Domain.Entities;

public class Wishlist: BaseEntityDal<Guid>
{
    /// <summary>
    /// Название вишлиста
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание вишлиста
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// id пользователя, которому принадлежит вишлист
    /// </summary>
    public required Guid UserId { get; set; }
}