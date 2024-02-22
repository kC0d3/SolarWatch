using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Services.Repository;

public class SolarRepository : ISolarRepository
{
    private static string _connectionString = ConnectionString.GetTestConnectionString();

    private static readonly DbContextOptions<SolarWatchContext> _options =
        new DbContextOptionsBuilder<SolarWatchContext>()
            .UseSqlServer(_connectionString)
            .Options;

    public async Task<SolarDto?> GetSolarRespByDateAndIdAsync(DateTime date, int id)
    {
        await using var dbContext = new SolarWatchContext(_options);
        var solarData = dbContext.Solars.Include(s => s.City).FirstOrDefault(s => s.Date == date && s.CityId == id);
        return new SolarDto
        {
            Id = solarData.Id,
            Sunrise = solarData.Sunrise,
            Sunset = solarData.Sunset,
            Date = solarData.Date,
            City = new CityResp
            {
                Id = solarData.City.Id,
                Name = solarData.City.Name,
                Country = solarData.City.Country,
                State = solarData.City.State,
                Latitude = solarData.City.Latitude,
                Longitude = solarData.City.Longitude
            },
            CityId = solarData.CityId
        };
    }

    public async Task<Solar> GetSolarByDateAndIdAsync(DateTime date, int id)
    {
        await using var dbContext = new SolarWatchContext(_options);
        return dbContext.Solars.Include(s => s.City).FirstOrDefault(s => s.Date == date && s.CityId == id);
    }

    public IEnumerable<Solar> GetAll()
    {
        using var dbContext = new SolarWatchContext(_options);
        return dbContext.Solars.ToList();
    }

    public void Add(Solar solar)
    {
        using var dbContext = new SolarWatchContext(_options);
        dbContext.Add(solar);
        dbContext.SaveChanges();
    }

    public void Delete(Solar solar)
    {
        using var dbContext = new SolarWatchContext(_options);
        dbContext.Remove(solar);
        dbContext.SaveChanges();
    }

    public void Update(Solar solar)
    {
        using var dbContext = new SolarWatchContext(_options);
        dbContext.Update(solar);
        dbContext.SaveChanges();
    }
}