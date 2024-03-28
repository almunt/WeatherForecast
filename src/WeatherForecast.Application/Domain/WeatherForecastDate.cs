namespace WeatherForecast.Application.Domain;

//TODO: add invariants
public sealed record WeatherForecastDate(
    DateOnly Date,
    IReadOnlyCollection<WeatherForecastHour> Hours);
