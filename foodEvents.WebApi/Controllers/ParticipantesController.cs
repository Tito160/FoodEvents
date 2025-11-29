using FoodEvents.Biblioteca;
using foodEvents.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace foodEvents.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantesController : ControllerBase
{
    private readonly FoodEventsService _service;

    public ParticipantesController(FoodEventsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParticipanteDto>>> Get()
    {
        var participantes = await _service.ObtenerParticipantesAsync();
        return Ok(participantes.Select(p => p.ToDto()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ParticipanteDto>> GetById(int id)
    {
        var participante = await _service.ObtenerParticipantePorIdAsync(id);
        if (participante is null)
        {
            return NotFound();
        }

        return Ok(participante.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CrearParticipanteDto dto)
    {
        var participante = new Participante
        {
            NombreCompleto = dto.NombreCompleto,
            CorreoElectronico = dto.CorreoElectronico,
            Telefono = dto.Telefono,
            DocumentoIdentidad = dto.DocumentoIdentidad,
            RestriccionAlimentaria = dto.RestriccionAlimentaria
        };

        var resultado = await _service.CrearParticipanteAsync(participante);

        if (!resultado.Exito)
        {
            return BadRequest(new { errores = resultado.Errores });
        }

        return CreatedAtAction(nameof(GetById), new { id = resultado.Valor!.Id }, resultado.Valor!.ToDto());
    }
}

public class CrearParticipanteDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public string? RestriccionAlimentaria { get; set; }
}
