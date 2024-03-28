namespace WeatherForecast.Application.Domain;

//TODO: add invariants
public record WeatherForecastHour(
    DateTime DateTime,
    double Temperature,
    double WindSpeed);
