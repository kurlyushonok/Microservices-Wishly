using Application.Dto;
using Domain.Logic.Interfaces;
using MassTransit;
using Saga.Contracts;

namespace Api.Consumers;

public class CreateDefaultWishlistConsumer : IConsumer<CreateDefaultWishlist>
{
    private readonly IWishlistService _wishlistService;

    public CreateDefaultWishlistConsumer(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }
    
    public async Task Consume(ConsumeContext<CreateDefaultWishlist> context)
    {
        try
        {
            var createDto = new WishlistCreateDto
            {
                Title = "My first wishlist",
                Description = "It's my first default wishlist",
                CreatedAt = DateTime.Now
            };

            var wishlist = await _wishlistService.CreateAsync(createDto, context.Message.UserId);

            await context.Publish(new DefaultWishlistCreated
            {
                CorrelationId = context.Message.CorrelationId,
                UserId = context.Message.UserId,
                WishlistId = wishlist.Id
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