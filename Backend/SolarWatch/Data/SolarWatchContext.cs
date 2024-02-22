using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Solar> Solars { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=yourStrong(!)Password;Encrypt=false;");
    }*/
}