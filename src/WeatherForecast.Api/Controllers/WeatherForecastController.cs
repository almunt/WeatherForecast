using EnsureThat;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Application.Domain;
using WeatherForecast.Application.Exceptions;
using WeatherForecast.Application.Queries;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("forecast")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(IMediator mediator)
    {
        EnsureArg.IsNotNull(mediator, nameof(mediator));

        this.mediator = mediator;
    }

    private readonly IMediator mediator;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string city,
        [FromQuery] string country,
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        //TODO: use FluentValidation to validate queries
        if (DateTime.UtcNow.Date > new DateTime(date, new TimeOnly(0)))
        {
            return this.BadRequest("Historical data are not available due to free API Keys");
        }

        var request = new GetWeatherForecast(city, country, date);

        IReadOnlyCollection<ProviderWeatherForecast> forecast;
        try
        {
            forecast = await this.mediator.Send(request, cancellationToken);
        }
        catch (GeoCoordinateNotFoundException exception)
        {
            //TODO: handle exceptions in UnhandledExceptionHandler and convert them to ProblemDetails
            return this.BadRequest(exception.Message);
        }

        //TODO: can be use factory, but not in the test task
        var response = new WeatherForecastResponse(
            date,
            OpenWeatherMap: this.ToWeatherForecastItems(forecast.ElementAt(0)),
            WeatherApiCom: this.ToWeatherForecastItems(forecast.ElementAt(1)),
            VisualCrossing: this.ToWeatherForecastItems(forecast.ElementAt(2)));

        return this.Ok(response);
    }

    //TODO: Use AutoMapper or Mapster
    private IReadOnlyCollection<WeatherForecastItem> ToWeatherForecastItems(ProviderWeatherForecast weatherForecast)
        => weatherForecast.Hours.Select(hour => new WeatherForecastItem(hour.DateTime, hour.Temperature, hour.WindSpeed)).ToArray();
}
