using DotNetEnv;

namespace SolarWatch.Data;

public static class ConnectionString
{
    public static string GetConnectionString()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, "..", "..", ".env");
        Env.Load(dotenv);
        return
            $"Server=tcp:solarwatch.database.windows.net,1433;Initial Catalog=solarwatchdb;Persist Security Info=False;User ID=kC0d3;Password={"yourStrong(!)Password"};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //$"Server={Environment.GetEnvironmentVariable("DBHOST")},{Environment.GetEnvironmentVariable("DBPORT")};Database={Environment.GetEnvironmentVariable("DBNAME")};User Id={Environment.GetEnvironmentVariable("DBUSER")};Password={Environment.GetEnvironmentVariable("DBPASSWORD")};Encrypt=false;";
    }

    public static string GetTestConnectionStringForFactory()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, "..", "..", "..", "..", "..", ".env");
        Env.Load(dotenv);
        return
            //$"Server=tcp:solarwatch.database.windows.net,1433;Initial Catalog=solarwatchdb;Persist Security Info=False;User ID=kC0d3;Password={"yourStrong(!)Password"};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            $"Server={Environment.GetEnvironmentVariable("DBHOST")},{Environment.GetEnvironmentVariable("DBPORT")};Database={Environment.GetEnvironmentVariable("TESTDBNAME")};User Id={Environment.GetEnvironmentVariable("DBUSER")};Password={Environment.GetEnvironmentVariable("DBPASSWORD")};Encrypt=false;";
    }

    public static string GetTestConnectionString()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, "..", "..", ".env");
        Env.Load(dotenv);
        return
            $"Server={Environment.GetEnvironmentVariable("DBHOST")},{Environment.GetEnvironmentVariable("DBPORT")};Database={Environment.GetEnvironmentVariable("TESTDBNAME")};User Id={Environment.GetEnvironmentVariable("DBUSER")};Password={Environment.GetEnvironmentVariable("DBPASSWORD")};Encrypt=false;";
    }
}