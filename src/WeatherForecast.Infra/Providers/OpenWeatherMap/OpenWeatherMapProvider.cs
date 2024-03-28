using EnsureThat;
using Weather.NET;
using Weather.NET.Enums;
using WeatherForecast.Application;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Infra.Providers.OpenWeatherMap;

internal sealed class OpenWeatherMapProvider : IWeatherForecastProvider
{
    public OpenWeatherMapProvider(string apiKey)
    {
        EnsureArg.IsNotNull(apiKey, nameof(apiKey));

        this.weatherClient = new WeatherClient(apiKey);
    }

    private readonly WeatherClient weatherClient;

    public async Task<WeatherForecastDate> GetAsync(GeoCoordinate geoCoordinate, DateOnly date, CancellationToken cancellationToken)
    {
        var response = await this.weatherClient.GetForecastAsync(
                geoCoordinate.Latitude,
                geoCoordinate.Longitude,
                measurement: Measurement.Metric)
            .ConfigureAwait(false);

        var forecast = response
            .Where(model => model.AnalysisDate.Date == new DateTime(date, new TimeOnly(0)))
            .ToArray();

        var hours = forecast.Any()
            ? forecast.Select(model => new WeatherForecastHour(model.AnalysisDate, model.Main.Temperature, model.Wind.Speed)).ToArray()
            : Array.Empty<WeatherForecastHour>();

        return new WeatherForecastDate(date, hours);
    }
}
