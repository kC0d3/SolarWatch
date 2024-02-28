using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
        var usersContext = scope.ServiceProvider.GetRequiredService<UsersContext>();

        if (!databaseContext.Database.CanConnect())
        {
            await databaseContext.Database.MigrateAsync();
        }

        if (!usersContext.Database.CanConnect())
        {
            await usersContext.Database.MigrateAsync();
        }
    }
}