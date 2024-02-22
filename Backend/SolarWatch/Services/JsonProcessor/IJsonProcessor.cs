using SolarWatch.Models;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    Solar ProcessSolarDataFromAPI(string data, DateTime date, int id);
    City ProcessCityDataFromAPI(string data);
}