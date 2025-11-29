using FoodEvents.Biblioteca;
using foodEvents.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace foodEvents.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChefsController : ControllerBase
{
    private readonly FoodEventsService _service;

    public ChefsController(FoodEventsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChefDto>>> Get()
    {
        var chefs = await _service.ObtenerChefsAsync();
        return Ok(chefs.Select(c => c.ToDto()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChefDto>> GetById(int id)
    {
        var chef = await _service.ObtenerChefPorIdAsync(id);
        if (chef is null)
        {
            return NotFound();
        }

        return Ok(chef.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CrearChefDto dto)
    {
        var chef = new Chef
        {
            NombreCompleto = dto.NombreCompleto,
            EspecialidadCulinaria = dto.EspecialidadCulinaria,
            Nacionalidad = dto.Nacionalidad,
            AniosExperiencia = dto.AniosExperiencia,
            CorreoElectronico = dto.CorreoElectronico,
            TelefonoContacto = dto.TelefonoContacto
        };

        var resultado = await _service.CrearChefAsync(chef);

        if (!resultado.Exito)
        {
            return BadRequest(new { errores = resultado.Errores });
        }

        return CreatedAtAction(nameof(GetById), new { id = resultado.Valor!.Id }, resultado.Valor!.ToDto());
    }
}

public class CrearChefDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string EspecialidadCulinaria { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public int AniosExperiencia { get; set; }
    public string CorreoElectronico { get; set; } = string.Empty;
    public string TelefonoContacto { get; set; } = string.Empty;
}
