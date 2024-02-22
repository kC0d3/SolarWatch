using System.Net.Http.Headers;

namespace SolarWatch.Services.CityDataProvider;

public interface ICityDataProvider
{
    Task<string> GetCityDataFromOuterApi(string city);
}