using Dal.DTO;
using Logic.Interfaces;
using MassTransit;
using Saga.Contracts;

namespace Api.Consumers;

public class CreateUserConsumer : IConsumer<CreateUser>
{
    private readonly IUserService _userService;

    public CreateUserConsumer(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        try
        {
            var userDto = new UserRegisterDto
            {
                Username = context.Message.Username,
                Password = context.Message.Password,
                ConfirmPassword = context.Message.ConfirmPassword,
            };

            var user = await _userService.RegisterAsync(userDto);

            await context.Publish(new UserCreated
            {
                CorrelationId = context.Message.CorrelationId,
                UserId = user.Id,
                Username = user.Username
            });
        }
        catch (Exception ex)
        {
            await context.Publish(new UserCreationFailed
            {
                CorrelationId = context.Message.CorrelationId,
                Reason = ex.Message
            });
        }
    }
}