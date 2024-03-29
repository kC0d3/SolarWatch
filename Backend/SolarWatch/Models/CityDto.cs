namespace SolarWatch.Models;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string? State { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public IEnumerable<SolarResp> Solars { get; set; }
}