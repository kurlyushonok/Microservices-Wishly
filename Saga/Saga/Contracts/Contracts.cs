namespace Saga.Contracts;

/// <summary>
/// Команда начала процесса
/// </summary>
public record StartUserCreation
{
    public required Guid CorrelationId { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}

/// <summary>
/// Команда создания пользователя
/// </summary>
public record CreateUser
{
    public required Guid CorrelationId { get; init; }
    public required string Username { get; init; } = string.Empty;
    
    public required string Password { get; init; } = string.Empty;
    public required string ConfirmPassword { get; init; } = string.Empty;
}

/// <summary>
/// Событие создания пользователя
/// </summary>
public record UserCreated
{
    public required Guid CorrelationId { get; init; }
    public required Guid UserId { get; init; }
    public required string Username { get; init; } = string.Empty;
}

/// <summary>
/// Команда создания вишлиста по умолчанию
/// </summary>
public record CreateDefaultWishlist
{
    public required Guid CorrelationId { get; init; }
    public required Guid UserId { get; init; }
    public required string Username { get; init; } = string.Empty;
    public required DateTime CreatedAt { get; init; }
}

/// <summary>
/// Событие создания вишлиста по умолчанию
/// </summary>
public record DefaultWishlistCreated
{
    public required Guid CorrelationId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid WishlistId { get; init; }
}

/// <summary>
/// Событие успешного завершения
/// </summary>
public record UserCreationCompleted
{
    public required Guid CorrelationId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid WishlistId { get; init; }
}

/// <summary>
/// Событие неуспешного завершения
/// </summary>
public record UserCreationFailed
{
    public required Guid CorrelationId { get; init; }
    public required string Reason { get; init; } = string.Empty;
}

/// <summary>
/// Откат
/// </summary>
public record CompensateUserCreation
{
    public required Guid CorrelationId { get; init; }
    public required Guid UserId { get; init; }
    public required string Reason { get; init; } = string.Empty;
}