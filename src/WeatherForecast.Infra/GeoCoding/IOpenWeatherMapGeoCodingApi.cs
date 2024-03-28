using Refit;

namespace WeatherForecast.Infra.GeoCoding;

internal interface IOpenWeatherMapGeoCodingApi
{
    [Get("/geo/1.0/direct")]
    Task<IReadOnlyCollection<OpenWeatherMapGeoCodingItem>> Search([Query] string q, CancellationToken cancellationToken);

    #region Contracts

    public record OpenWeatherMapGeoCodingItem(
        string Name,
        double Lat,
        double Lon,
        string Country,
        string State);

    #endregion
}
