using Domain.Interfaces;
using Domain.Logic.Interfaces;
using Infrastucture.Data;
using Infrastucture.Repositories;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Infrastucture;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IWishlistRepository, WishlistRepository>();

        return services;
    }
}