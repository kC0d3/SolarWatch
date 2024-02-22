using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Data;

public static class DataExtensions
{
    public static async Task InitializeTestDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var solarWatchContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
        var usersContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
        await solarWatchContext.Database.MigrateAsync();
        await usersContext.Database.MigrateAsync();
    }
}