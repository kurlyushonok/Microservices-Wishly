using Logic.Interfaces;
using Logic.Services;

namespace Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
            
        return services;
    }
}