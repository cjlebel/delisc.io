using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace RedisCaching;

public class RedisCaching : IRedisCaching
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCaching([FromKeyedServices("cache")] IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }
}
