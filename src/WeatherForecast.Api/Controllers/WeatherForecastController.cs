using EnsureThat;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Application.Domain;
using WeatherForecast.Application.Queries;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(IMediator mediator)
    {
        EnsureArg.IsNotNull(mediator, nameof(mediator));

        this.mediator = mediator;
    }

    private readonly IMediator mediator;

    [HttpGet]
    public async Task<WeatherForecastResponse> Get(
        [FromQuery] string city,
        [FromQuery] string country,
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        var request = new GetWeatherForecast(city, country, date);

        var result = await this.mediator.Send(request, cancellationToken);

        return new WeatherForecastResponse(
            date,
            OpenWeatherMap: this.ToWeatherForecastItems(result.ElementAt(0)),
            WeatherApiCom: this.ToWeatherForecastItems(result.ElementAt(1)));
    }

    //TODO: Use AutoMapper
    private IReadOnlyCollection<WeatherForecastItem> ToWeatherForecastItems(WeatherForecastDate weatherForecast)
        => weatherForecast.Hours.Select(hour => new WeatherForecastItem(hour.DateTime, hour.Temperature, hour.WindSpeed)).ToArray();
}
