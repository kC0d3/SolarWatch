using SolarWatch.Models;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SolarDataProvider;

namespace SolarWatch.Services.SolarService;

public class SolarService : ISolarService
{
    private readonly ILogger<SolarService> _logger;
    private readonly ISolarRepository _solarRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ISolarDataProvider _solarDataProvider;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarService(ILogger<SolarService> logger, ISolarRepository solarRepository, ICityRepository cityRepository,
        ISolarDataProvider solarDataProvider, ICityDataProvider cityDataProvider,
        IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _solarRepository = solarRepository;
        _cityRepository = cityRepository;
        _solarDataProvider = solarDataProvider;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<Solar> ProcessGetSolarData(string city, DateTime date)
    {
        var cityData = await _cityRepository.GetCityByNameIncludesSolarAsync(city);
        if (cityData == null)
        {
            _logger.LogWarning("City data missing from database.");
            var cityDataFromApi = await _cityDataProvider.GetCityDataFromOuterApi(city);
            var cityObject = _jsonProcessor.ProcessCityDataFromAPI(cityDataFromApi);
            _cityRepository.Add(cityObject);
            cityData = await _cityRepository.GetCityByNameIncludesSolarAsync(city);
            _logger.LogInformation("Get city data from database.");
        }

        var solarData = await _solarRepository.GetSolarByDateAndIdAsync(date, cityData.Id);

        if (solarData == null)
        {
            _logger.LogWarning("Solar data missing from database.");
            var solarDataFromApi = await _solarDataProvider.GetSolarDataFromOutApi(city, date);
            var solarObject = _jsonProcessor.ProcessSolarDataFromAPI(solarDataFromApi, date, cityData.Id);
            _solarRepository.Add(solarObject);
            solarData = await _solarRepository.GetSolarByDateAndIdAsync(date, cityData.Id);
            _logger.LogInformation("Get solar data from database.");
        }

        _logger.LogInformation("Return solar data from database.");
        return solarData;
    }
}