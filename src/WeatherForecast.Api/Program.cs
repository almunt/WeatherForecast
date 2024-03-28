using Microsoft.OpenApi.Models;
using WeatherForecast.Application;
using WeatherForecast.Infra.Extensions;
using WeatherForecast.Infra.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    }));

var openWeatherMapOptions = builder.Configuration.GetSection("OpenWeatherMap")
    .Get<WeatherForecastApiOptions>();
var weatherApiOptions = builder.Configuration.GetSection("WeatherApi")
    .Get<WeatherForecastApiOptions>();

builder.Services.AddGeoCodingService(openWeatherMapOptions!);
builder.Services.AddOpenWeatherMapProvider(openWeatherMapOptions!);
builder.Services.AddWeatherApiProvider(weatherApiOptions!);

builder.Services.AddCache();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IWeatherForecastProvider>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
