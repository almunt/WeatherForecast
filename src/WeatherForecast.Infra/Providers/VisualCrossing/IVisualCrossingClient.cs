using Refit;

namespace WeatherForecast.Infra.Providers.VisualCrossing;

public interface IVisualCrossingClient
{
    [Get("/VisualCrossingWebServices/rest/services/timeline/{geo}/next14days")]
    Task<WeatherForecast> GetForecastAsync([AliasAs("geo")] string geo, [Query] string unitGroup, CancellationToken cancellationToken);

    #region Contracts

    public record WeatherForecast(IReadOnlyCollection<DailyWeatherForecast> Days);

    public record DailyWeatherForecast(DateOnly Datetime, IReadOnlyCollection<HourlyWeatherForecast>? Hours);

    public record HourlyWeatherForecast(TimeOnly Datetime, double Temp, double Windspeed);

    #endregion
}
