using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolarWatch.Data;
using SolarWatch.Models;
using SolarWatch.Services.Repository;

namespace SolarWatch.Services.Repository;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchContext _dbContext;

    public CityRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<City?> GetCityByNameIncludesSolarAsync(string city)
    {
        return _dbContext.Cities.Include(c => c.Solars).FirstOrDefault(c => c.Name == city);
    }

    public async Task<CityDto?> GetCityDtoByNameIncludesSolarRespAsync(string city)
    {
        var cityData = _dbContext.Cities.Include(c => c.Solars).FirstOrDefault(c => c.Name == city);
        return new CityDto
        {
            Id = cityData.Id,
            Name = cityData.Name,
            Country = cityData.Country,
            State = cityData.State,
            Latitude = cityData.Latitude,
            Longitude = cityData.Longitude,
            Solars = cityData.Solars.Select(s => new SolarResp()
            {
                Id = s.Id,
                Sunrise = s.Sunrise,
                Sunset = s.Sunset,
                Date = s.Date,
                CityId = s.CityId
            })
        };
    }

    public async Task<City?> GetByNameAsync(string city)
    {
        return _dbContext.Cities.FirstOrDefault(c => c.Name == city);
    }

    public IEnumerable<City> GetAll()
    {
        return _dbContext.Cities.ToList();
    }

    public void Add(City city)
    {
        _dbContext.Add(city);
        _dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        _dbContext.Remove(city);
        _dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        _dbContext.Update(city);
        _dbContext.SaveChanges();
    }
}