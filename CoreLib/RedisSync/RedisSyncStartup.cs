using Microsoft.Extensions.DependencyInjection;
using CoreLib.DistributedLockLogic;

namespace CoreLib.RedisSync;

public static class RedisSyncStartup
{
    public static IServiceCollection AddRedisDistributedSemaphore(this IServiceCollection services, string? connectionString)
    {
        services.AddSingleton<IDistributedSemaphoreFactory>(
            new RedisDistributedSemaphoreFactory(connectionString));
        
        return services;
    }
}