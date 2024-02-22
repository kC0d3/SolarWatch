/*using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Services.CityService;
using SolarWatch.Services.SolarService;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/SolarWatch")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICityService _cityService;
    private readonly ISolarService _solarService;
    private readonly UserManager<IdentityUser> _userManager;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICityService cityService,
        ISolarService solarService, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _cityService = cityService;
        _solarService = solarService;
        _userManager = userManager;
    }

    [HttpGet("GetSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarDto>> GetSolarData([Required] string city, [Required] DateTime date)
    {
        try
        {
            return Ok(await _solarService.ProcessGetSolarData(city, date));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet("GetCityData"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<City>> GetCityData([Required] string city)
    {
        try
        {
            return Ok(await _cityService.ProcessGetCityData(city));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }
}*/

