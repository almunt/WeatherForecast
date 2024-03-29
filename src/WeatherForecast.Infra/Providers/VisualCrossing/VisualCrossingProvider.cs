using EnsureThat;
using WeatherForecast.Application;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Infra.Providers.VisualCrossing;

internal sealed class VisualCrossingProvider : IWeatherForecastProvider
{
    public VisualCrossingProvider(IVisualCrossingClient apiClient)
    {
        EnsureArg.IsNotNull(apiClient, nameof(apiClient));

        this.apiClient = apiClient;
    }

    private readonly IVisualCrossingClient apiClient;

    public async Task<ProviderWeatherForecast> GetAsync(GeoCoordinate geoCoordinate, DateOnly date, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(geoCoordinate, nameof(geoCoordinate));

        var query = $"{geoCoordinate.Latitude},{geoCoordinate.Longitude}";

        var response = await this.apiClient.GetForecastAsync(query, "metric", cancellationToken).ConfigureAwait(false);

        var forecast = response.Days.FirstOrDefault(day => day.Datetime ==date);

        var hours = forecast is null || forecast.Hours is null
            ? Array.Empty<HourlyWeatherForecast>()
            : forecast.Hours
                .Select(hour => new HourlyWeatherForecast(new DateTime(forecast.Datetime, hour.Datetime), hour.Temp, hour.Windspeed))
                .ToArray();

        return new ProviderWeatherForecast(date, hours);
    }
}
