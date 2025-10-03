namespace Domain.Entities;

public class BaseEntityDal<T>
{
    /// <summary>
    /// Генерируемый уникальный идентификационный номер
    /// </summary>
    public required T Id { get; init; }
}