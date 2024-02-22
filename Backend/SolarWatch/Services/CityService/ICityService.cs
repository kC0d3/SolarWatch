using SolarWatch.Models;

namespace SolarWatch.Services.CityService;

public interface ICityService
{
    Task<City> ProcessGetCityData(string city);
}