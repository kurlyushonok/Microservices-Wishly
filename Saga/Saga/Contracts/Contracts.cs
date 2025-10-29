namespace Saga.Contracts;

/// <summary>
/// Команда начала процесса
/// </summary>
public record StartUserCreation
{
    public Guid CorrelationId { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}

/// <summary>
/// Команда создания пользователя
/// </summary>
public record CreateUser
{
    public Guid CorrelationId { get; init; }
    public string Username { get; init; } = string.Empty;
    
    public string Password { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
}

/// <summary>
/// Событие создания пользователя
/// </summary>
public record UserCreated
{
    public Guid CorrelationId { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
}

/// <summary>
/// Команда создания вишлиста по умолчанию
/// </summary>
public record CreateDefaultWishlist
{
    public Guid CorrelationId { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Событие создания вишлиста по умолчанию
/// </summary>
public record DefaultWishlistCreated
{
    public Guid CorrelationId { get; init; }
    public Guid UserId { get; init; }
    public Guid WishlistId { get; init; }
}

/// <summary>
/// Событие успешного завершения
/// </summary>
public record UserCreationCompleted
{
    public Guid CorrelationId { get; init; }
    public Guid UserId { get; init; }
    public Guid WishlistId { get; init; }
}

/// <summary>
/// Событие неуспешного завершения
/// </summary>
public record UserCreationFailed
{
    public Guid CorrelationId { get; init; }
    public string Reason { get; init; } = string.Empty;
}

/// <summary>
/// Откат
/// </summary>
public record CompensateUserCreation
{
    public Guid CorrelationId { get; init; }
    public Guid UserId { get; init; }
    public string Reason { get; init; } = string.Empty;
}