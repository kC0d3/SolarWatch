using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services.JsonProcessor;

public class JsonProcessor : IJsonProcessor
{
    public Solar ProcessSolarDataFromAPI(string data, DateTime date, int id)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        Solar solar = new Solar
        {
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString(),
            Date = date,
            CityId = id
        };

        return solar;
    }

    public City ProcessCityDataFromAPI(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        City cityData = new City()
        {
            Name = json.RootElement[0].GetProperty("name").GetString(),
            Country = json.RootElement[0].GetProperty("country").GetString(),
            State = json.RootElement[0].TryGetProperty("state", out var stateProperty)
                ? stateProperty.GetString()
                : null,
            Latitude = json.RootElement[0].GetProperty("lat").GetDouble(),
            Longitude = json.RootElement[0].GetProperty("lon").GetDouble(),
            Solars = null,
        };

        return cityData;
    }
}