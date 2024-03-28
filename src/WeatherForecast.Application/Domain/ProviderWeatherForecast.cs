namespace WeatherForecast.Application.Domain;

//TODO: add invariants
public sealed record ProviderWeatherForecast(
    DateOnly Date,
    IReadOnlyCollection<HourlyWeatherForecast> Hours);
