using System.Net;
using System.Net.Http.Headers;
using Azure.Core;
using RequestHeaders = Microsoft.AspNetCore.Http.Headers.RequestHeaders;

namespace SolarWatch.Services.CityDataProvider;

public class CityDataProvider : ICityDataProvider
{
    private readonly ILogger<CityDataProvider> _logger;

    public CityDataProvider(ILogger<CityDataProvider> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetCityDataFromOuterApi(string city)
    {
        var apiKey = "951126ccb19f7fcd4abc6d054950684a";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}