using MediatR;
using WeatherForecast.Application.Domain;

namespace WeatherForecast.Application.Queries;

public record GetWeatherForecast(string City, string Country, DateOnly Date)
    : IRequest<IReadOnlyCollection<WeatherForecastDate>>;
