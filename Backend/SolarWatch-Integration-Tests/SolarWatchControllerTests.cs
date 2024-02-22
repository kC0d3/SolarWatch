using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using SolarWatch.Contracts;
using SolarWatch.Models;

namespace SolarWatch_Integration_Tests;

public class SolarWatchControllerTests
{
    [Fact]
    public async Task Register()
    {
        //Arrange
        var application = new SolarWatchWebApplicationFactory();
        var user = new RegistrationRequest("user1@user1.com", "user1", "123456");
        var client = application.CreateClient();

        //Act
        var response = await client.PostAsJsonAsync("/api/auth/register", user);

        //Assert
        response.EnsureSuccessStatusCode();
        var userResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        userResponse?.UserName.Should().Be(user.Username);
        userResponse?.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task Login()
    {
        //Arrange
        var application = new SolarWatchWebApplicationFactory();
        var user = new AuthRequest("admin", "admin123");
        var client = application.CreateClient();

        //Act
        var response = await client.PostAsJsonAsync("/api/auth/login", user);

        //Assert
        response.EnsureSuccessStatusCode();
        var userResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        userResponse?.UserName.Should().Be(user.UserName);
        userResponse?.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateCityData()
    {
        //Arrange
        var application = new SolarWatchWebApplicationFactory();
        var city = "London";
        var user = new AuthRequest("admin", "admin123");
        var client = application.CreateClient();
        var resp = await client.PostAsJsonAsync("/api/auth/login", user);
        var userResponse = await resp.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse?.Token);

        //Act
        var content = new StringContent("", Encoding.UTF8, "text/plain");
        var response = await client.PostAsync($"/api/cities?city={city}", content);

        //Assert
        response.EnsureSuccessStatusCode();
        var cityResponse = await response.Content.ReadFromJsonAsync<CityDto>();
        cityResponse?.Id.Should().BePositive();
        cityResponse?.Name.Should().Be($"{city}");
    }

    [Fact]
    public async Task CreateSolarDataByDateAndTime()
    {
        //Arrange
        var application = new SolarWatchWebApplicationFactory();
        var city = "London";
        var date = "2023-08-20";
        var user = new AuthRequest("admin", "admin123");
        var client = application.CreateClient();
        var resp = await client.PostAsJsonAsync("/api/auth/login", user);
        var userResponse = await resp.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse?.Token);

        //Act
        var content = new StringContent("", Encoding.UTF8, "text/plain");
        await client.PostAsync($"/api/cities?city={city}", content);
        var response = await client.PostAsync($"/api/solars/?city={city}&date={date}", content);

        //Assert
        response.EnsureSuccessStatusCode();
        var solarResponse = await response.Content.ReadFromJsonAsync<SolarDto>();
        solarResponse?.Id.Should().BePositive();
        solarResponse?.City.Name.Should().Be($"{city}");
    }

    /*[Fact]
    public async Task GetSolarDataByDateAndTime()
    {
        //Arrange
        var application = new SolarWatchWebApplicationFactory();
        var city = "London";
        var date = "2023-08-20";
        var regUser = new RegistrationRequest("user1@user1.com", "user1", "123456");
        var user = new AuthRequest("user1", "123456");
        var admin = new AuthRequest("admin", "admin123");
        var content = new StringContent("", Encoding.UTF8, "text/plain");

        var client = application.CreateClient();
        var adminResp = await client.PostAsJsonAsync("/api/auth/login", admin);
        var adminResponse = await adminResp.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminResponse?.Token);
        
        await client.PostAsync($"/api/cities?city={city}", content);
        await client.PostAsync($"/api/solars?city={city}&date={date}", content);
        
        await client.PostAsJsonAsync("/api/auth/register", regUser);
        var userResp = await client.PostAsJsonAsync("/api/auth/login", user);
        var userResponse = await userResp.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse?.Token);

        //Act
        var response = await client.GetAsync($"/api/solars/{city}&{date}");


        //Assert
        response.EnsureSuccessStatusCode();
        var solarResponse = await response.Content.ReadFromJsonAsync<SolarDto>();
        solarResponse?.Id.Should().BePositive();
        solarResponse?.City.Name.Should().Be($"{city}");
    }*/
}
