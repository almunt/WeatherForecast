# Wheather Forecast

The app provides weather forecast data from three different sources.

# Limits

Not all sources support historical data for free accounts, so I decided not to support them at all. It is assumed that the user will request a weather forecast within the next two weeks. However, OpenWeatherMap provides weather forecasts for a week, at least for free accounts. If the source cannot provide a weather forecast for the specified date, then the user will receive an empty array. To keep things simpler, from all the variety of weather forecast data, only temperature and wind speed are returned. The metric measurement system is used.

## Azure

The app was deployed on [Azure](https://weatherforecastapi20240328214631.azurewebsites.net/swagger/index.html).
