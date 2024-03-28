using WeatherForecast.Application.Domain;

namespace WeatherForecast.Application;

public interface IWeatherForecastCache
{
    Task<IReadOnlyCollection<ProviderWeatherForecast>?> GetAsync(string key, CancellationToken cancellationToken);

    Task SetAsync(
        string key,
        IReadOnlyCollection<ProviderWeatherForecast> value,
        CancellationToken cancellationToken);
}
