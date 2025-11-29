using FoodEvents.Biblioteca;
using foodEvents.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace foodEvents.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly FoodEventsService _service;

    public ReservasController(FoodEventsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaDto>>> Get()
    {
        var reservas = await _service.ObtenerReservasAsync();
        return Ok(reservas.Select(r => r.ToDto()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReservaDto>> GetById(int id)
    {
        var reserva = await _service.ObtenerReservaPorIdAsync(id);
        if (reserva is null)
        {
            return NotFound();
        }

        return Ok(reserva.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CrearReservaDto dto)
    {
        try
        {
            var reserva = new Reserva
            {
                ParticipanteId = dto.ParticipanteId,
                EventoGastronomicoId = dto.EventoGastronomicoId,
                YaPago = dto.YaPago,
                MetodoPago = dto.MetodoPago,
                EstadoReserva = dto.EstadoReserva
            };

            var resultado = await _service.CrearReservaAsync(reserva);

            if (!resultado.Exito)
            {
                return BadRequest(new { errores = resultado.Errores });
            }

            return Ok(resultado.Valor!.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { errores = new[] { "Error inesperado al crear la reserva.", ex.Message, ex.InnerException?.Message } });
        }
    }

    [HttpPost("{id:int}/cancelar")]
    public async Task<ActionResult> Cancelar(int id)
    {
        var ok = await _service.CancelarReservaAsync(id);
        if (!ok)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public class CrearReservaDto
{
    public int ParticipanteId { get; set; }
    public int EventoGastronomicoId { get; set; }
    public bool YaPago { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public EstadoReserva EstadoReserva { get; set; }
}
