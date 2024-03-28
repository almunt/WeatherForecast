using EnsureThat;
using MediatR;
using WeatherForecast.Application.Domain;
using WeatherForecast.Application.Exceptions;
using WeatherForecast.Application.Queries;

namespace WeatherForecast.Application.QueryHandlers;

internal sealed class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecast, IReadOnlyCollection<ProviderWeatherForecast>>
{
    public GetWeatherForecastHandler(
        IGeoCodingService geoCodingService,
        IEnumerable<IWeatherForecastProvider> providers,
        IWeatherForecastCache cache)
    {
        EnsureArg.IsNotNull(geoCodingService, nameof(geoCodingService));
        EnsureArg.IsNotNull(providers, nameof(providers));
        EnsureArg.IsNotNull(cache, nameof(cache));

        this.geoCodingService = geoCodingService;
        this.providers = providers;
        this.cache = cache;
    }

    private readonly IGeoCodingService geoCodingService;
    private readonly IEnumerable<IWeatherForecastProvider> providers;
    private readonly IWeatherForecastCache cache;

    public async Task<IReadOnlyCollection<ProviderWeatherForecast>> Handle(GetWeatherForecast request, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        var cacheKey = $"{request.Date}{request.City}{request.Country}";

        var forecasts = await this.cache.GetAsync(cacheKey, cancellationToken).ConfigureAwait(false);
        if (forecasts is not null)
        {
            return forecasts;
        }

        var geoCoordinate = await this.geoCodingService
            .GetGeoCoordinatesAsync(request.City, request.Country, cancellationToken)
            .ConfigureAwait(false);
        if (geoCoordinate is null)
        {
            throw new GeoCoordinateNotFoundException($"GEO location of {request.City}, {request.Country} not found");
        }

        var tasks = this.providers
            .Select(provider => provider.GetAsync(geoCoordinate, request.Date, cancellationToken));

        forecasts = await Task.WhenAll(tasks).ConfigureAwait(false);

        await this.cache.SetAsync(cacheKey, forecasts, cancellationToken).ConfigureAwait(false);

        return forecasts;
    }
}
