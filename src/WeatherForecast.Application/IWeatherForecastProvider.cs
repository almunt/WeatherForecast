﻿using WeatherForecast.Application.Domain;

namespace WeatherForecast.Application;

public interface IWeatherForecastProvider
{
    Task<ProviderWeatherForecast> GetAsync(
        GeoCoordinate geoCoordinate,
        DateOnly date,
        CancellationToken cancellationToken);
}
