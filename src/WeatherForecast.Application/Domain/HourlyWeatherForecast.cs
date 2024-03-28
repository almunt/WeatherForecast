namespace WeatherForecast.Application.Domain;

//TODO: add invariants
public record HourlyWeatherForecast(
    DateTime DateTime,
    double Temperature,
    double WindSpeed);
