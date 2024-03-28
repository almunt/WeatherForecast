using EnsureThat;
using WeatherForecast.Application;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Infra.Providers.WeatherApiProvider;

internal sealed class WeatherApiProvider : IWeatherForecastProvider
{
    public WeatherApiProvider(IWeatherApiClient apiClient)
    {
        EnsureArg.IsNotNull(apiClient, nameof(apiClient));

        this.apiClient = apiClient;
    }

    private const int ForecastDays = 30;

    private readonly IWeatherApiClient apiClient;

    public async Task<ProviderWeatherForecast> GetAsync(GeoCoordinate geoCoordinate, DateOnly date, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(geoCoordinate, nameof(geoCoordinate));

        var query = $"{geoCoordinate.Latitude},{geoCoordinate.Longitude}";

        var response = await this.apiClient.GetForecastAsync(query, ForecastDays, cancellationToken).ConfigureAwait(false);

        var forecast = response.Forecast.Forecastday.FirstOrDefault(day => day.Date == new DateTime(date, new TimeOnly(0)));

        var hours = forecast is null
            ? Array.Empty<HourlyWeatherForecast>()
            : forecast.Hour.Select(hour => new HourlyWeatherForecast(DateTime.Parse(hour.Time), hour.Temp_c, hour.Wind_kph)).ToArray();

        return new ProviderWeatherForecast(date, hours);
    }
}
