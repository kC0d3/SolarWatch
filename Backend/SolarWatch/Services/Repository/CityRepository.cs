using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolarWatch.Data;
using SolarWatch.Models;
using SolarWatch.Services.Repository;

namespace SolarWatch.Services.Repository;

public class CityRepository : ICityRepository
{
    private static readonly string _connectionString = ConnectionString.GetTestConnectionString();

    private static readonly DbContextOptions<SolarWatchContext> _options =
        new DbContextOptionsBuilder<SolarWatchContext>()
            .UseSqlServer(_connectionString)
            .Options;


    public async Task<City?> GetCityByNameIncludesSolarAsync(string city)
    {
        await using var dbContext = new SolarWatchContext(_options);
        return dbContext.Cities.Include(c => c.Solars).FirstOrDefault(c => c.Name == city);
    }

    public async Task<CityDto?> GetCityDtoByNameIncludesSolarRespAsync(string city)
    {
        await using var dbContext = new SolarWatchContext(_options);
        var cityData = dbContext.Cities.Include(c => c.Solars).FirstOrDefault(c => c.Name == city);
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
        await using var dbContext = new SolarWatchContext(_options);
        return dbContext.Cities.FirstOrDefault(c => c.Name == city);
    }

    public IEnumerable<City> GetAll()
    {
        using var dbContext = new SolarWatchContext(_options);
        return dbContext.Cities.ToList();
    }

    public void Add(City city)
    {
        using var dbContext = new SolarWatchContext(_options);
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        using var dbContext = new SolarWatchContext(_options);
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        using var dbContext = new SolarWatchContext(_options);
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}