namespace CoreLib.Entities;

public class BaseEntityDal<T>
{
    public required T Id { get; init; }
}