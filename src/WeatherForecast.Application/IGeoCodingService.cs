using WeatherForecast.Application.Domain;

namespace WeatherForecast.Application;

public interface IGeoCodingService
{
    public Task<GeoCoordinate?> GetGeoCoordinatesAsync(
        string city,
        string? country,
        CancellationToken cancellationToken);
}
