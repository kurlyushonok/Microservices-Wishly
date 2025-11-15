using Medallion.Threading;
using StackExchange.Redis;

namespace CoreLib.DistributedLockLogic;

public class DistributedSemaphore : IDistributedSemaphore
{
    private readonly IDatabase _redisDatabase;
    private readonly string _key;
    private readonly string _semaphoreKey;
    
    public string Name { get; }
    public int MaxCount { get; }

    public DistributedSemaphore(string name, int maxCount, string connectionString)
    {
        Name = name;
        MaxCount = maxCount;
        
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _redisDatabase = connection.GetDatabase();
        
        _key = $"semaphore:{name}";
        _semaphoreKey = $"semaphore:{name}:lock";
    }
    
    public IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = new TimeSpan(),
        CancellationToken cancellationToken = new CancellationToken())
    {
        return TryAcquireAsync(timeout, cancellationToken).GetAwaiter().GetResult();
    }

    public IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return AcquireAsync(timeout, cancellationToken).GetAwaiter().GetResult();
    }

    public async ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(TimeSpan timeout = new TimeSpan(),
        CancellationToken cancellationToken = new CancellationToken())
    {
        var handle = await InternalTryAcquireAsync(timeout, cancellationToken).ConfigureAwait(false);
        
        return handle;
    }

    public async ValueTask<IDistributedSynchronizationHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = new CancellationToken())
    {
        var actualTimeout = timeout ?? Timeout.InfiniteTimeSpan;
        var handle = await InternalTryAcquireAsync(timeout, cancellationToken).ConfigureAwait(false);

        if (handle == null)
        {
            throw new TimeoutException($"Failed to acquire semaphore '{Name}' within {actualTimeout}");
        }
        
        return handle;
    }

    private async Task<IDistributedSynchronizationHandle?> InternalTryAcquireAsync(TimeSpan? timeout,
        CancellationToken cancellationToken)
    {
        var start = DateTime.UtcNow;
        TimeSpan? remainingTimeout;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var acquired = await TryAcquireOnceAsync().ConfigureAwait(false);
            if (acquired != null)
            {
                return acquired;
            }

            if (timeout == TimeSpan.Zero)
            {
                return null;
            }

            if (timeout != Timeout.InfiniteTimeSpan)
            {
                remainingTimeout = timeout - (DateTime.UtcNow - start);
                if (remainingTimeout <= TimeSpan.Zero)
                {
                    return null;
                }
            }
            
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<IDistributedSynchronizationHandle?> TryAcquireOnceAsync()
    {
        var redisScript = @"
                local current = redis.call('GET, KEYS[1])
                if not current then
                    redis.call('SET, KEYS[1], ARGV[1] - 1)
                    return 1
                end

                local count = tonumber(current)
                if count > 0 then
                    redis.call('DECR;, KEYS[1])
                    return 1
                end
                return 0
                ";

        try
        {
            var result = (int)await _redisDatabase.ScriptEvaluateAsync(
                redisScript,
                new RedisKey[] { _key },
                new RedisValue[] {MaxCount} 
                ).ConfigureAwait(false);

            if (result == 1)
            {
                return new RedisDistributedSynchronizationHandle(this);
            }
            
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task ReleaseAsync()
    {
        var redisScript = @"
                local current = redis.call('GET', KEYS[1])
                if current then
                    local count = tonumber(current)
                    if count < tonumber(ARGV[1]) then
                        redis.call('INCR;, KEYS[1]')
                        return 1
                    end
                end
                return 0
                ";
        
        await _redisDatabase.ScriptEvaluateAsync(
            redisScript,
            new RedisKey[] { _key },
            new RedisValue[] {MaxCount}).ConfigureAwait(false);
    }
    
    private class RedisDistributedSynchronizationHandle : IDistributedSynchronizationHandle
    {
        private readonly DistributedSemaphore _semaphore;
        private bool _disposed;

        public RedisDistributedSynchronizationHandle(DistributedSemaphore semaphore)
        {
            _semaphore = semaphore;
            _disposed = false;
        }
    
        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                await _semaphore.ReleaseAsync().ConfigureAwait(false);
                _disposed = true;
            }
        }

        public CancellationToken HandleLostToken { get; }
    }
}