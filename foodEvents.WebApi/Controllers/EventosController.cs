using FoodEvents.Biblioteca;
using foodEvents.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace foodEvents.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly FoodEventsService _service;

    public EventosController(FoodEventsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventoDto>>> Get()
    {
        var eventos = await _service.ObtenerEventosAsync();
        return Ok(eventos.Select(e => e.ToDto()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventoDto>> GetById(int id)
    {
        var evento = await _service.ObtenerEventoPorIdAsync(id);
        if (evento is null)
        {
            return NotFound();
        }

        return Ok(evento.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CrearEventoDto dto)
    {
        var evento = new EventoGastronomico
        {
            Nombre = dto.Nombre,
            DescripcionDetallada = dto.DescripcionDetallada,
            TipoEvento = dto.TipoEvento,
            Modalidad = dto.Modalidad,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            CapacidadMaxima = dto.CapacidadMaxima,
            PrecioPorEntrada = dto.PrecioPorEntrada,
            Ubicacion = dto.Ubicacion,
            UrlAccesoVirtual = dto.UrlAccesoVirtual,
            ChefId = dto.ChefId
        };

        var resultado = await _service.CrearEventoAsync(evento);

        if (!resultado.Exito)
        {
            return BadRequest(new { errores = resultado.Errores });
        }

        return CreatedAtAction(nameof(GetById), new { id = resultado.Valor!.Id }, resultado.Valor);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _service.EliminarEventoAsync(id);
        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public class CrearEventoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string DescripcionDetallada { get; set; } = string.Empty;
    public TipoEventoGastronomico TipoEvento { get; set; }
    public ModalidadEvento Modalidad { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int CapacidadMaxima { get; set; }
    public decimal PrecioPorEntrada { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public string? UrlAccesoVirtual { get; set; }
    public int ChefId { get; set; }
}
