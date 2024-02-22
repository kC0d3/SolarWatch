namespace SolarWatch.Models;

public class SolarDto
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public DateTime Date { get; set; }
    
    public CityResp City { get; set; }
    public int CityId { get; set; }
}