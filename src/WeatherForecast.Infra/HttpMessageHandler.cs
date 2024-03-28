using EnsureThat;

namespace WeatherForecast.Infra;

internal sealed class HttpMessageHandler : DelegatingHandler
{
    public HttpMessageHandler(string apiKeyParamName, string apiKeyParamValue)
    {
        EnsureArg.IsNotEmptyOrWhiteSpace(apiKeyParamName, nameof(apiKeyParamName));
        EnsureArg.IsNotEmptyOrWhiteSpace(apiKeyParamValue, nameof(apiKeyParamValue));

        this.apiKeyParamName = apiKeyParamName;
        this.apiKeyParamValue = apiKeyParamValue;

        this.InnerHandler = new HttpClientHandler();
    }

    private readonly string apiKeyParamName;
    private readonly string apiKeyParamValue;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        request.RequestUri = new Uri($"{request.RequestUri}&{this.apiKeyParamName}={this.apiKeyParamValue}");

        return base.SendAsync(request, cancellationToken);
    }
}
