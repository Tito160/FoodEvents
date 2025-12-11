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

        // DEBUG: revisar en logs o breakpoint
        var primeraCantidad = eventos.FirstOrDefault()?.Reservas?.Count ?? 0; // ¿> 0?
                                                                              // opcional: return Ok(eventos); // devuelve entidades completas para inspeccionar JSON crudo

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

    [HttpPost("{id:int}/participantes")]
    public async Task<ActionResult> AgregarParticipantes(int id, [FromBody] List<int> participanteIds)
    {
        var resultado = await _service.AgregarParticipantesAEventoAsync(id, participanteIds);

        if (!resultado.Exito)
            return BadRequest(new { errores = resultado.Errores });

        var reservasDto = resultado.Valor.ReservasCreadas.Select(r => r.ToDto()).ToList();

        return Ok(new
        {
            mensaje = resultado.Valor.Mensaje,
            confirmados = resultado.Valor.Confirmados,
            enEspera = resultado.Valor.EnEspera,
            totalAgregados = resultado.Valor.ReservasCreadas.Count,
            reservas = reservasDto
        });
    }

    [HttpGet("modalidades")]
    public ActionResult<IEnumerable<object>> GetModalidadesEvento()
    {
        var modalidades = Enum.GetValues(typeof(ModalidadEvento))
                            .Cast<ModalidadEvento>()
                            .Select(m => new
                            {
                                valor = (int)m,
                                nombre = m.ToString()
                            })
                            .ToList();

        return Ok(modalidades);
    }

    [HttpGet("tipos")]
    public ActionResult<IEnumerable<object>> GetTiposEventoGastronomico()
    {
        var tipos = Enum.GetValues(typeof(TipoEventoGastronomico))
                        .Cast<TipoEventoGastronomico>()
                        .Select(t => new
                        {
                            valor = (int)t,
                            nombre = t.ToString() // Cata, Feria, Clase, CenaTematica
                        })
                        .ToList();

        return Ok(tipos);
    }

    [HttpGet("{id:int}/cupos-disponibles")]
    public async Task<ActionResult<object>> GetCuposDisponibles(int id)
    {
        var evento = await _service.ObtenerEventoPorIdAsync(id);

        if (evento is null)
            return NotFound(new { mensaje = "Evento no encontrado" });

        // Contar solo reservas confirmadas
        var reservasConfirmadas = evento.Reservas?
            .Count(r => r.EstadoReserva == EstadoReserva.Confirmada) ?? 0;

        var cuposOcupados = reservasConfirmadas;
        var cuposDisponibles = evento.CapacidadMaxima - cuposOcupados;
        var estaLleno = cuposDisponibles <= 0;

        return Ok(new
        {
            eventoId = evento.Id,
            nombreEvento = evento.Nombre,
            capacidadMaxima = evento.CapacidadMaxima,
            cuposOcupados,
            cuposDisponibles = cuposDisponibles < 0 ? 0 : cuposDisponibles,
            estado = estaLleno ? "COMPLETO" : cuposDisponibles == 1 ? "Último cupo disponible" : "Hay cupos disponibles",
            mensaje = estaLleno 
                ? "El evento está completo. No se pueden agregar más inscripciones confirmadas." 
                : cuposDisponibles == 1 
                    ? "¡Queda solo 1 cupo! Apurate a inscribirte." 
                    : $"Quedan {cuposDisponibles} cupos disponibles de {evento.CapacidadMaxima}."
        });
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
