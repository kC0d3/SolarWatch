using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Services.Repository;

public class SolarRepository : ISolarRepository
{
    private readonly SolarWatchContext _dbContext;

    public SolarRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SolarDto?> GetSolarRespByDateAndIdAsync(DateTime date, int id)
    {
        var solarData = _dbContext.Solars.Include(s => s.City).FirstOrDefault(s => s.Date == date && s.CityId == id);
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
        return _dbContext.Solars.Include(s => s.City).FirstOrDefault(s => s.Date == date && s.CityId == id);
    }

    public IEnumerable<Solar> GetAll()
    {
        return _dbContext.Solars.ToList();
    }

    public void Add(Solar solar)
    {
        _dbContext.Add(solar);
        _dbContext.SaveChanges();
    }

    public void Delete(Solar solar)
    {
        _dbContext.Remove(solar);
        _dbContext.SaveChanges();
    }

    public void Update(Solar solar)
    {
        _dbContext.Update(solar);
        _dbContext.SaveChanges();
    }
}