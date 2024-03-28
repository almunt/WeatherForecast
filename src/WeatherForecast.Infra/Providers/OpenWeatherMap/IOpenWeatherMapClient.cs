using Refit;

namespace WeatherForecast.Infra.Providers.OpenWeatherMap;

internal interface IOpenWeatherMapClient
{
    [Get("/data/2.5/forecast")]
    Task<WeatherForecast> GetForecastAsync([Query] double lat, [Query] double lon, [Query] string units, CancellationToken cancellationToken);

    #region Contracts

    public record WeatherForecast(IReadOnlyCollection<WeatherForecastHour> List);

    public record WeatherForecastHour(string Dt_txt, WeatherForecastMain Main, WeatherForecastWind Wind);

    public record WeatherForecastMain(double Temp);

    public record WeatherForecastWind(double Speed);

    #endregion
}
