using EnsureThat;
using WeatherForecast.Application;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Infra.Providers.OpenWeatherMap;

internal sealed class OpenWeatherMapProvider : IWeatherForecastProvider
{
    public OpenWeatherMapProvider(IOpenWeatherMapClient apiClient)
    {
        EnsureArg.IsNotNull(apiClient, nameof(apiClient));

        this.apiClient = apiClient;
    }

    private readonly IOpenWeatherMapClient apiClient;

    public async Task<ProviderWeatherForecast> GetAsync(GeoCoordinate geoCoordinate, DateOnly date, CancellationToken cancellationToken)
    {
        var response = await this.apiClient.GetForecastAsync(
                geoCoordinate.Latitude,
                geoCoordinate.Longitude,
                "metric",
                cancellationToken)
            .ConfigureAwait(false);

        var forecast = response.List
            .Where(hour => DateTime.Parse(hour.Dt_txt).Date == new DateTime(date, new TimeOnly(0)))
            .ToArray();

        var hours = forecast.Any()
            ? forecast.Select(hour => new HourlyWeatherForecast(DateTime.Parse(hour.Dt_txt), hour.Main.Temp, hour.Wind.Speed)).ToArray()
            : Array.Empty<HourlyWeatherForecast>();

        return new ProviderWeatherForecast(date, hours);
    }
}
