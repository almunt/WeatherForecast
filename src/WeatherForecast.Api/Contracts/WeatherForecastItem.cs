namespace WeatherForecast.Api.Contracts;

public sealed record WeatherForecastItem(
    DateTime Time,
    double Temperature,
    double WindSpeed);
