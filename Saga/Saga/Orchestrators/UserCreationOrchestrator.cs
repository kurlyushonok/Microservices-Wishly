using MassTransit;

namespace Saga.Orchestrators;

public class UserCreationOrchestrator : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = "Initial";
    
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public Guid? WishlistId { get; set; }
    
    public DateTime CreatedAt { get; init; }
    public string? FailureReason { get; set; }
}