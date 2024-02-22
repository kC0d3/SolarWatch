using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolarWatch.Data;

namespace SolarWatch_Integration_Tests;

internal class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connString = ConnectionString.GetTestConnectionStringForFactory();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<SolarWatchContext>));
            services.RemoveAll(typeof(DbContextOptions<UsersContext>));

            services.AddDbContext<SolarWatchContext>(options =>
                options.UseSqlServer(_connString));
            services.AddDbContext<UsersContext>(options =>
                options.UseSqlServer(_connString));

            var solarWatchContext = CreateSolarWatchContext(services);
            solarWatchContext.Database.EnsureDeleted();
            var usersContext = CreateUsersContext(services);
            usersContext.Database.EnsureDeleted();
        });
    }

    private static SolarWatchContext CreateSolarWatchContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
        return dbContext;
    }

    private static UsersContext CreateUsersContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
        return dbContext;
    }
}