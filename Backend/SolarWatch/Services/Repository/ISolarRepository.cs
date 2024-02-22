using SolarWatch.Models;

namespace SolarWatch.Services.Repository;

public interface ISolarRepository
{
    Task<SolarDto?> GetSolarRespByDateAndIdAsync(DateTime date, int id);
    Task<Solar> GetSolarByDateAndIdAsync(DateTime date, int id);
    IEnumerable<Solar> GetAll();
    void Add(Solar solar);
    void Delete(Solar solar);
    void Update(Solar solar);
}