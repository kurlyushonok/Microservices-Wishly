using Medallion.Threading;

namespace CoreLib.DistributedLockLogic;

public interface IDistributedSemaphoreFactory
{
    IDistributedSemaphore Create(string name, int maxCount);
}

public class RedisDistributedSemaphoreFactory : IDistributedSemaphoreFactory
{
    private readonly string _connectionString;

    public RedisDistributedSemaphoreFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDistributedSemaphore Create(string name, int maxCount)
    {
        return new DistributedSemaphore(name, maxCount, _connectionString);
    }
}