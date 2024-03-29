using Refit;

namespace WeatherForecast.Infra.Providers.WeatherApiProvider;

internal interface IWeatherApiClient
{
    [Get("/v1/forecast.json")]
    Task<WeatherForecast> GetForecastAsync([Query] string q, [Query] int days, CancellationToken cancellationToken);

    #region Contracts

    public record WeatherForecast(WeatherForecastDays Forecast);

    public record WeatherForecastDays(IReadOnlyCollection<DailyWeatherForecast> Forecastday);

    public record DailyWeatherForecast(DateTime Date, IReadOnlyCollection<HourlyWeatherForecast> Hour);

    public record HourlyWeatherForecast(string Time, double Temp_c, double Wind_kph);

    #endregion
}

