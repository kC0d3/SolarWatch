namespace SolarWatch.Services.Authentication;

public interface IRolesService
{
    void AddRoles(WebApplication app);
    void AddAdmin(WebApplication app);
}