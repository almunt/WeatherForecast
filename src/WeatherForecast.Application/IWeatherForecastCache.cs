using WeatherForecast.Application.Domain;

namespace WeatherForecast.Application;

public interface IWeatherForecastCache
{
    Task<IReadOnlyCollection<WeatherForecastDate>?> GetAsync(string key, CancellationToken cancellationToken);

    Task SetAsync(
        string key,
        IReadOnlyCollection<WeatherForecastDate> value,
        CancellationToken cancellationToken);
}
