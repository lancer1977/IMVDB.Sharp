using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models;

namespace PolyhydraGames.IMVDB.Test.Fakes;

public class FakeCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public FakeCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = redis.GetDatabase();
    }
    public async Task<string> GetString(string key, Func<Task<string>> func)
    {
        var redisResult = await _db.StringGetAsync(key);
        if (redisResult.HasValue)
        {
            return redisResult;
        }

        var result = await func();
        _db.StringSet(key, result);
        return result;
    }

    public async Task<T> Get<T>(string key, Func<Task<T>> func)
    {
        var redisResult = await _db.StringGetAsync(key);
        if (redisResult.HasValue)
        {
            return JsonConvert.DeserializeObject<T>(redisResult);
        }
        else
        {
            var result = await func();
            _db.StringSet(key, result.ToJson());
            return result;
        }
    }
    public Task<string> GetString(string key, Func<Task<string>> func, TimeSpan? ttl, bool forceRefresh = false)
    {
        throw new NotImplementedException();
    }
    public Task<T> Get<T>(string key, Func<Task<T>> func, TimeSpan? ttl, bool forceRefresh = false)
    {
        throw new NotImplementedException();
    }

    public async Task Clear(string key)
    {
        _db.StringGetDelete(key);
    }
}