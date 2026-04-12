using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResidentialComplexesController : ControllerBase
{
    private readonly IResidentialComplexService _residentialComplexService;

    public ResidentialComplexesController(IResidentialComplexService residentialComplexService)
    {
        _residentialComplexService = residentialComplexService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string lang = "az")
    {
        var result = await _residentialComplexService.GetAllAsync(lang);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] string lang = "az")
    {
        var result = await _residentialComplexService.GetByIdAsync(id, lang);

        if (!result.IsSuccess)
            return NotFound(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }
}
