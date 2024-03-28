using WeatherForecast.Application.Domain;

namespace WeatherForecast.Application;

public interface IWeatherForecastProvider
{
    Task<WeatherForecastDate> GetAsync(
        GeoCoordinate geoCoordinate,
        DateOnly date,
        CancellationToken cancellationToken);
}
