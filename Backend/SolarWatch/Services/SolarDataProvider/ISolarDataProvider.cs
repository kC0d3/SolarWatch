using System.Net.Http.Headers;

namespace SolarWatch.Services.SolarDataProvider;

public interface ISolarDataProvider
{
    Task<string> GetSolarDataFromOutApi(string city, DateTime date);
}