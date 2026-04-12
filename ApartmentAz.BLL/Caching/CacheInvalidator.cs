using ApartmentAz.BLL.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ApartmentAz.BLL.Caching;

public class CacheInvalidator : ICacheInvalidator
{
    private readonly IMemoryCache _cache;
    private static readonly HashSet<string> _trackedKeys = [];
    private static readonly Lock _lock = new();

    public CacheInvalidator(IMemoryCache cache)
    {
        _cache = cache;
    }

    public static void TrackKey(string key)
    {
        lock (_lock)
        {
            _trackedKeys.Add(key);
        }
    }

    public void InvalidateListings()
    {
        RemoveByPrefix(CacheKeys.ListingsPrefix);
    }

    public void InvalidateLocations()
    {
        RemoveByPrefix(CacheKeys.CitiesPrefix);
        RemoveByPrefix(CacheKeys.DistrictsPrefix);
        RemoveByPrefix(CacheKeys.MetrosPrefix);
    }

    private void RemoveByPrefix(string prefix)
    {
        List<string> keysToRemove;
        lock (_lock)
        {
            keysToRemove = _trackedKeys.Where(k => k.StartsWith(prefix)).ToList();
            foreach (var key in keysToRemove)
                _trackedKeys.Remove(key);
        }

        foreach (var key in keysToRemove)
            _cache.Remove(key);
    }
}
