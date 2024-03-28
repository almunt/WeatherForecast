using Refit;

namespace WeatherForecast.Infra.Providers.WeatherApiProvider;

internal interface IWeatherApiClient
{
    [Get("/v1/forecast.json")]
    Task<WeatherForecast> Search([Query] string q, [Query] int days, CancellationToken cancellationToken);

    #region Contracts

    public record WeatherForecast(WeatherForecastDays Forecast);

    public record WeatherForecastDays(IReadOnlyCollection<WeatherForecastDay> Forecastday);

    public record WeatherForecastDay(DateTime Date, IReadOnlyCollection<WeatherForecastHour> Hour);

    public record WeatherForecastHour(string Time, double Temp_c, double Wind_kph);

    #endregion
}

