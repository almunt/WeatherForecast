namespace WeatherForecast.Api.Contracts;

public sealed record WeatherForecastResponse(
    DateOnly Date,
    IReadOnlyCollection<WeatherForecastItem> OpenWeatherMap,
    IReadOnlyCollection<WeatherForecastItem> WeatherApiCom,
    IReadOnlyCollection<WeatherForecastItem> VisualCrossing);
