using SolarWatch.Models;

namespace SolarWatch.Services.SolarService;

public interface ISolarService
{
   Task<Solar> ProcessGetSolarData(string city, DateTime date);
}