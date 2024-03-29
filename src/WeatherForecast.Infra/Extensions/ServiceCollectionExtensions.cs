using EnsureThat;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WeatherForecast.Application;
using WeatherForecast.Infra.Cache;
using WeatherForecast.Infra.GeoCoding;
using WeatherForecast.Infra.Options;
using WeatherForecast.Infra.Providers.OpenWeatherMap;
using WeatherForecast.Infra.Providers.VisualCrossing;
using WeatherForecast.Infra.Providers.WeatherApiProvider;

namespace WeatherForecast.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeoCodingService(this IServiceCollection services, WeatherForecastApiOptions options)
    {
        EnsureArg.IsNotNull(services, nameof(services));
        EnsureArg.IsNotNull(options, nameof(options));

        services.AddScoped(_ =>
            {
                var settings = new RefitSettings
                {
                    HttpMessageHandlerFactory = () => new HttpMessageHandler("appid", options.ApiKey)
                };

                return RestService.For<IOpenWeatherMapGeoCodingApi>(options.ApiUrl.ToString(), settings);
            });

        services.AddScoped<IGeoCodingService, GeoCodingService>();

        return services;
    }

    public static IServiceCollection AddOpenWeatherMapProvider(this IServiceCollection services, WeatherForecastApiOptions options)
    {
        EnsureArg.IsNotNull(services, nameof(services));
        EnsureArg.IsNotNull(options, nameof(options));

        services.AddScoped(_ => RestService.For<IOpenWeatherMapClient>(options.ApiUrl.ToString(), BuildRefitSettings("appid", options.ApiKey)));

        services.AddScoped<IWeatherForecastProvider, OpenWeatherMapProvider>();

        return services;
    }


    public static IServiceCollection AddWeatherApiProvider(this IServiceCollection services,WeatherForecastApiOptions options)
    {
        EnsureArg.IsNotNull(services, nameof(services));
        EnsureArg.IsNotNull(options, nameof(options));

        services.AddScoped(_ => RestService.For<IWeatherApiClient>(options.ApiUrl.ToString(), BuildRefitSettings("key", options.ApiKey)));

        services.AddScoped<IWeatherForecastProvider, WeatherApiProvider>();

        return services;
    }

    public static IServiceCollection AddVisualCrossingProvider(this IServiceCollection services, WeatherForecastApiOptions options)
    {
        EnsureArg.IsNotNull(services, nameof(services));
        EnsureArg.IsNotNull(options, nameof(options));

        services.AddScoped(_ => RestService.For<IVisualCrossingClient>(options.ApiUrl.ToString(), BuildRefitSettings("key", options.ApiKey)));

        services.AddScoped<IWeatherForecastProvider, VisualCrossingProvider>();

        return services;
    }


    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        EnsureArg.IsNotNull(services, nameof(services));

        services.AddMemoryCache();

        services.AddScoped<IWeatherForecastCache, WeatherForecastCache>();

        return services;
    }

    private static RefitSettings BuildRefitSettings(string apiKeyParam, string apiKey)
    {
        var settings = new RefitSettings
        {
            HttpMessageHandlerFactory = () => new HttpMessageHandler(apiKeyParam, apiKey)
        };

        return settings;
    }
}
