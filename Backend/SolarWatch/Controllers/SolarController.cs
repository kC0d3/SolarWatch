using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Services;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SolarDataProvider;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/solars")]
public class SolarController : ControllerBase
{
    private readonly ILogger<SolarController> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly ISolarRepository _solarRepository;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISolarDataProvider _solarDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarController(ILogger<SolarController> logger, ICityRepository cityRepository,
        ISolarRepository solarRepository,
        ICityDataProvider cityDataProvider, ISolarDataProvider solarDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityRepository = cityRepository;
        _solarRepository = solarRepository;
        _cityDataProvider = cityDataProvider;
        _solarDataProvider = solarDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("{city}&{date}")]
    public async Task<ActionResult<SolarDto>> GetSolarData([Required] string city, [Required] DateTime date)
    {
        try
        {
            var cityData = await _cityRepository.GetByNameAsync(city);
            return Ok(await _solarRepository.GetSolarRespByDateAndIdAsync(date, cityData.Id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<SolarDto>> CreateSolarData([Required] string city, [Required] DateTime date)
    {
        try
        {
            var cityData = await _cityRepository.GetByNameAsync(city);
            if (cityData == null)
            {
                return NotFound();
            }

            var solarDataFromDb = await _solarRepository.GetSolarByDateAndIdAsync(date, cityData.Id);
            if (solarDataFromDb != null)
            {
                return BadRequest();
            }

            var solarDataFromApi = await _solarDataProvider.GetSolarDataFromOutApi(city, date);
            var solarData = _jsonProcessor.ProcessSolarDataFromAPI(solarDataFromApi, date, cityData.Id);
            _solarRepository.Add(solarData);
            return Ok(await _solarRepository.GetSolarRespByDateAndIdAsync(date, cityData.Id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{city}&{date}")]
    public async Task<ActionResult<CityDto>> UpdateSolarData([Required] string city, [Required] DateTime date,
        [FromBody] SolarDto solarDto)
    {
        try
        {
            var cityData = await _cityRepository.GetByNameAsync(city);
            if (cityData == null)
            {
                return NotFound();
            }

            var solarData = await _solarRepository.GetSolarByDateAndIdAsync(date, cityData.Id);
            if (solarData == null)
            {
                return NotFound();
            }

            solarData.Date = solarDto.Date;
            solarData.Sunrise = solarDto.Sunrise;
            solarData.Sunset = solarDto.Sunset;
            _solarRepository.Update(solarData);

            return Ok(solarData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{city}&{date}")]
    public async Task<ActionResult<CityDto>> DeleteSolarData([Required] string city, [Required] DateTime date)
    {
        try
        {
            var cityData = await _cityRepository.GetByNameAsync(city);
            if (cityData == null)
            {
                return NotFound();
            }

            var solarData = await _solarRepository.GetSolarByDateAndIdAsync(date, cityData.Id);
            if (solarData == null)
            {
                return NotFound();
            }

            _solarRepository.Delete(solarData);
            return Ok(solarData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }
}