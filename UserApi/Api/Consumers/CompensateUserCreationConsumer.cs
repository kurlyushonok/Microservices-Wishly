using Logic.Interfaces;
using MassTransit;
using Saga.Contracts;

namespace Api.Consumers;

public class CompensateUserCreationConsumer : IConsumer<CompensateUserCreation>
{
    private readonly IUserService _userService;

    public CompensateUserCreationConsumer(IUserService userService, ILogger<CompensateUserCreationConsumer> logger)
    {
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<CompensateUserCreation> context)
    {
        try
        {
            await _userService.DeleteAsync(context.Message.UserId);
        }
        catch (Exception ex)
        {
            
        }
    }
}