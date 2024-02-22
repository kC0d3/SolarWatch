using System.Net;
using System.Net.Http.Headers;
using Azure.Core;
using Microsoft.AspNetCore.Http.Headers;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.Repository;
using RequestHeaders = Azure.Core.RequestHeaders;

namespace SolarWatch.Services.SolarDataProvider;

public class SolarDataProvider : ISolarDataProvider
{
    private readonly ILogger<SolarDataProvider> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;

    public SolarDataProvider(ILogger<SolarDataProvider> logger, ICityDataProvider cityDataProvider,
        IJsonProcessor jsonProcessor, ICityRepository cityRepository)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
        _cityRepository = cityRepository;
    }

    /*public async Task<string> GetSolarDataFromOutApi(string city, DateTime date)
    {
        var cityDataFromDb = await _cityRepository.GetByNameAsync(city);
        string url =
            $"https://api.sunrise-sunset.org/json?lat={cityDataFromDb.Latitude}&lng={cityDataFromDb.Longitude}date={date}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }*/

    public async Task<string> GetSolarDataFromOutApi(string city, DateTime date)
    {
        var cityDataFromDb = await _cityRepository.GetByNameAsync(city);
        string url;

        if (cityDataFromDb == null)
        {
            var cityDataFromApi =
                _jsonProcessor.ProcessCityDataFromAPI(await _cityDataProvider.GetCityDataFromOuterApi(city));
            url =
                $"https://api.sunrise-sunset.org/json?lat={cityDataFromApi.Latitude}&lng={cityDataFromApi.Longitude}date={date}";
            _logger.LogInformation("Use API data.");
        }
        else
        {
            url =
                $"https://api.sunrise-sunset.org/json?lat={cityDataFromDb.Latitude}&lng={cityDataFromDb.Longitude}date={date}";
            _logger.LogInformation("Use database data.");
        }

        using var client = new HttpClient();
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}