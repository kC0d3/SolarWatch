namespace SolarWatch.Models;

public class SolarResp
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public DateTime Date { get; set; }
    public int CityId { get; set; }
}