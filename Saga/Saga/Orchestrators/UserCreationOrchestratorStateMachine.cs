using MassTransit;
using Saga.Contracts;

namespace Saga.Orchestrators;

public class UserCreationOrchestratorStateMachine : MassTransitStateMachine<UserCreationOrchestrator>
{
    public State CreatingUser { get; private set; } = null!;
    public State CreatingWishlist { get; private set; } = null!;
    public State Compensating { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;
    
    public Event<StartUserCreation> StartUserCreation { get; private set; } = null!;
    public Event<UserCreated> UserCreated { get; private set; } = null!;
    public Event<DefaultWishlistCreated> DefaultWishlistCreated { get; private set; } = null!;
    public Event<UserCreationFailed> UserCreationFailed { get; private set; } = null!;
    
    public UserCreationOrchestratorStateMachine()
    {
        ConfigureStates();
        ConfigureEvents();
        ConfigureInitialBehavior();
        ConfigureCreatingUserState();
        ConfigureCreatingWishlistState();
        ConfigureFinalization();
    }

    private void ConfigureStates()
    {
        State(() => CreatingUser);
        State(() => CreatingWishlist);
        State(() => Compensating);
        State(() => Completed);
        State(() => Failed);
    }

    private void ConfigureEvents()
    {
        Event(() => StartUserCreation, e => e
            .CorrelateById(context => context.Message.CorrelationId));

        Event(() => UserCreated, e => e
            .CorrelateById(context => context.Message.CorrelationId));

        Event(() => DefaultWishlistCreated, e => e
            .CorrelateById(context => context.Message.CorrelationId));

        Event(() => UserCreationFailed, e => e
            .CorrelateById(context => context.Message.CorrelationId));
    }
    
    private void ConfigureInitialBehavior()
    {
        Initially(
            When(StartUserCreation)
                .Then(context =>
                {
                    context.Saga.Username = context.Message.Username;
                    context.Saga.Password = context.Message.Password;
                })
                .Publish(context => new CreateUser
                {
                    CorrelationId = context.Saga.CorrelationId,
                    Username = context.Saga.Username,
                    Password = context.Saga.Password,
                    ConfirmPassword = context.Saga.ConfirmPassword
                })
                .TransitionTo(CreatingUser)
        );
    }
    
    private void ConfigureCreatingUserState()
    {
        During(CreatingUser,
            When(UserCreated)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                })
                .Publish(context => new CreateDefaultWishlist
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId!.Value,
                    Username = context.Saga.Username
                })
                .TransitionTo(CreatingWishlist),

            When(UserCreationFailed)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                })
                .TransitionTo(Failed)
        );
    }
    
    private void ConfigureCreatingWishlistState()
    {
        During(CreatingWishlist,
            When(DefaultWishlistCreated)
                .Then(context =>
                {
                    context.Saga.WishlistId = context.Message.WishlistId;
                })
                .Publish(context => new UserCreationCompleted
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId!.Value,
                    WishlistId = context.Saga.WishlistId!.Value
                })
                .TransitionTo(Completed),

            When(UserCreationFailed)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                })
                .Publish(context => new CompensateUserCreation
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId!.Value,
                    Reason = context.Saga.FailureReason ?? "Unknown error"
                })
                .TransitionTo(Compensating)
        );
    }
    
    private void ConfigureFinalization()
    {
        SetCompletedWhenFinalized();
    }
}