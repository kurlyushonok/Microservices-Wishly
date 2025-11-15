using Microsoft.Extensions.DependencyInjection;
using CoreLib.DistributedLockLogic;

namespace CoreLib.RedisSync;

public class RedisSyncStartup
{
    public static IServiceCollection ConfigureServices(IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDistributedSemaphoreFactory>(
            new RedisDistributedSemaphoreFactory(connectionString));
        
        return services;
    }
}