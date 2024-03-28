using EnsureThat;

namespace WeatherForecast.Application.Domain;

public sealed record GeoCoordinate
{
    public GeoCoordinate(double latitude, double longitude)
    {
        EnsureArg.IsInRange(latitude, -180, 180, nameof(latitude));
        EnsureArg.IsInRange(longitude, -90, 90, nameof(longitude));

        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }
}
