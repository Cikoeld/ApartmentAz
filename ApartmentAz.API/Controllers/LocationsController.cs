using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IDistrictService _districtService;
    private readonly IMetroService _metroService;

    public LocationsController(
        ICityService cityService,
        IDistrictService districtService,
        IMetroService metroService)
    {
        _cityService = cityService;
        _districtService = districtService;
        _metroService = metroService;
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities([FromQuery] string lang = "az")
    {
        var result = await _cityService.GetAllAsync(lang);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("districts")]
    public async Task<IActionResult> GetDistricts([FromQuery] Guid cityId, [FromQuery] string lang = "az")
    {
        var result = await _districtService.GetByCityAsync(cityId, lang);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("metros")]
    public async Task<IActionResult> GetMetros([FromQuery] Guid cityId, [FromQuery] string lang = "az")
    {
        var result = await _metroService.GetByCityAsync(cityId, lang);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }
}
