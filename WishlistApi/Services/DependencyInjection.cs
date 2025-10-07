using Domain.Logic.Interfaces;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(
        this IServiceCollection services)
    {
        services.AddScoped<IWishlistService, WishlistService>();
            
        return services;
    }
}