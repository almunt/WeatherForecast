namespace WeatherForecast.Application.Exceptions;

public sealed class GeoCoordinateNotFoundException : Exception
{
    public GeoCoordinateNotFoundException(string message) : base(message)
    {
    }
}
