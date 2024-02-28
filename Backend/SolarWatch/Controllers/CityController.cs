using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Services;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/cities")]
public class CityController : ControllerBase
{
    private readonly ILogger<CityController> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public CityController(ILogger<CityController> logger, ICityRepository cityRepository,
        ICityDataProvider cityDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityRepository = cityRepository;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpGet("{city}")]
    public async Task<ActionResult<CityDto>> GetCityData([Required] string city)
    {
        try
        {
            var cityData = await _cityRepository.GetCityDtoByNameIncludesSolarRespAsync(city);
            if (cityData == null)
            {
                return NotFound();
            }

            return Ok(cityData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CityDto>> CreateCityData([Required] string city)
    {
        try
        {
            var cityDataFromDb = await _cityRepository.GetByNameAsync(city);
            if (cityDataFromDb != null)
            {
                return BadRequest();
            }

            var cityDataFromApi = await _cityDataProvider.GetCityDataFromOuterApi(city);
            var cityData = _jsonProcessor.ProcessCityDataFromAPI(cityDataFromApi);
            _cityRepository.Add(cityData);
            return Ok(await _cityRepository.GetCityDtoByNameIncludesSolarRespAsync(city));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{city}")]
    public async Task<ActionResult<City>> UpdateCityData([Required] string city, [FromBody] CityDto cityDto)
    {
        try
        {
            var cityData = await _cityRepository.GetByNameAsync(city);
            if (cityData == null)
            {
                return NotFound();
            }

            cityData.Name = cityDto.Name;
            cityData.Country = cityDto.Country;
            cityData.State = cityDto.State;
            cityData.Latitude = cityDto.Latitude;
            cityData.Longitude = cityDto.Longitude;
            _cityRepository.Update(cityData);

            return Ok(cityData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{city}")]
    public async Task<ActionResult<City>> UpdateCityData([Required] string city)
    {
        try
        {
            var cityData = await _cityRepository.GetByNameAsync(city);
            if (cityData == null)
            {
                return NotFound();
            }

            _cityRepository.Delete(cityData);
            return Ok(cityData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred!");
            return StatusCode(500, "An unexpected error occurred");
        }
    }
}