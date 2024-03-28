using EnsureThat;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WeatherForecast.Application;
using WeatherForecast.Infra.Cache;
using WeatherForecast.Infra.GeoCoding;
using WeatherForecast.Infra.Options;
using WeatherForecast.Infra.Providers.OpenWeatherMap;
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

        services.AddScoped<IWeatherForecastProvider>(_ => new OpenWeatherMapProvider(options.ApiKey));

        return services;
    }


    public static IServiceCollection AddWeatherApiProvider(this IServiceCollection services,WeatherForecastApiOptions options)
    {
        EnsureArg.IsNotNull(services, nameof(services));
        EnsureArg.IsNotNull(options, nameof(options));

        services.AddScoped(_ =>
        {
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => new HttpMessageHandler("key", options.ApiKey)
            };

            return RestService.For<IWeatherApiClient>(options.ApiUrl.ToString(), settings);
        });

        services.AddScoped<IWeatherForecastProvider, WeatherApiProvider>();

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        EnsureArg.IsNotNull(services, nameof(services));

        services.AddMemoryCache();

        services.AddScoped<IWeatherForecastCache, WeatherForecastCache>();

        return services;
    }
}
