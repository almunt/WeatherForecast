using EnsureThat;
using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.Application;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Infra.Cache;

//IMemoryCache is used. We can use Redis, if it needs.
internal sealed class WeatherForecastCache : IWeatherForecastCache
{
    public WeatherForecastCache(IMemoryCache memoryCache)
    {
        EnsureArg.IsNotNull(memoryCache, nameof(memoryCache));

        this.memoryCache = memoryCache;
    }

    private const int SlidingIntervalInMin = 5;

    private readonly IMemoryCache memoryCache;
    private readonly MemoryCacheEntryOptions options = new() { SlidingExpiration = TimeSpan.FromMinutes(SlidingIntervalInMin) };

    public Task<IReadOnlyCollection<ProviderWeatherForecast>?> GetAsync(string key, CancellationToken cancellationToken)
        => Task.FromResult(this.memoryCache.TryGetValue(key, out IReadOnlyCollection<ProviderWeatherForecast>? forecast)
            ? forecast
            : default);

    public Task SetAsync(string key, IReadOnlyCollection<ProviderWeatherForecast> value, CancellationToken cancellationToken)
    {
        this.memoryCache.Set(key, value, this.options);
        return Task.CompletedTask;
    }
}
