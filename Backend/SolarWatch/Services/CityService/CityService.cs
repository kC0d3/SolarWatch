using SolarWatch.Models;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.Repository;

namespace SolarWatch.Services.CityService;

public class CityService : ICityService
{
    private readonly ILogger<CityService> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public CityService(ILogger<CityService> logger, ICityRepository cityRepository, ICityDataProvider cityDataProvider,
        IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityRepository = cityRepository;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<City> ProcessGetCityData(string city)
    {
        var cityData = await _cityRepository.GetCityByNameIncludesSolarAsync(city);
        if (cityData == null)
        {
            _logger.LogWarning("City data missing from database.");
            var cityDataFromApi = _cityDataProvider.GetCityDataFromOuterApi(city);
            var cityObject = _jsonProcessor.ProcessCityDataFromAPI(cityDataFromApi.Result);
            _cityRepository.Add(cityObject);
            cityData = await _cityRepository.GetCityByNameIncludesSolarAsync(city);
        }

        _logger.LogInformation("Return city data from database.");
        return cityData;
    }
}