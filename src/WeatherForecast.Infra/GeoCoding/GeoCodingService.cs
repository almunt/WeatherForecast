using EnsureThat;
using WeatherForecast.Application;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Infra.GeoCoding;

internal sealed class GeoCodingService : IGeoCodingService
{
    public GeoCodingService(IOpenWeatherMapGeoCodingApi apiClient)
    {
        EnsureArg.IsNotNull(apiClient, nameof(apiClient));

        this.apiClient = apiClient;
    }

    private readonly IOpenWeatherMapGeoCodingApi apiClient;

    public async Task<GeoCoordinate?> GetGeoCoordinatesAsync(string city, string? country, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(city, nameof(city));

        var query = city;
        if (string.IsNullOrWhiteSpace(country))
        {
            query += $",{country}";
        }

        var coords = (await this.apiClient.Search(query, cancellationToken)
            .ConfigureAwait(false))
            .FirstOrDefault();

        return coords is null
            ? null
            : new GeoCoordinate(coords.Lat, coords.Lon);
    }
}
