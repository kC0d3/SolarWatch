using System.Text.Json.Serialization;
using SolarWatch.Models;

namespace SolarWatch;

public class Solar
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public DateTime Date { get; set; }

    public City City { get; set; }
    public int CityId { get; set; }
}