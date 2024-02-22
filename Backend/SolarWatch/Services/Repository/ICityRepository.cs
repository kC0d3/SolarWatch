using SolarWatch.Models;

namespace SolarWatch.Services.Repository;

public interface ICityRepository
{
    Task<City?> GetCityByNameIncludesSolarAsync(string city);
    Task<CityDto?> GetCityDtoByNameIncludesSolarRespAsync(string city);
    Task<City?> GetByNameAsync(string city);
    IEnumerable<City> GetAll();
    void Add(City city);
    void Delete(City city);
    void Update(City city);
}