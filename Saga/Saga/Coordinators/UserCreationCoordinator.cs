using MassTransit;
using Saga.Contracts;

namespace Saga.Coordinators;

public class UserCreationCoordinator : IConsumer<StartUserCreation>
{
    public async Task Consume(ConsumeContext<StartUserCreation> context)
    {
        try
        {
            await context.Publish(context.Message);
        }
        catch (Exception ex)
        {
            await context.Publish(new UserCreationFailed
            {
                CorrelationId = context.Message.CorrelationId,
                Reason = $"Failed to start saga: {ex.Message}"
            });
        }
    }
}