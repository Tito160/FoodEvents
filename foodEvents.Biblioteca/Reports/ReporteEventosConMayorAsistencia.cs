using Microsoft.EntityFrameworkCore;

namespace FoodEvents.Biblioteca.Reports;

public record EventoConAsistenciaDto(int EventoId, string Nombre, int CantidadReservasConfirmadas);

/// <summary>
/// Reporte que devuelve los eventos con mayor cantidad de reservas confirmadas.
/// </summary>
public class ReporteEventosConMayorAsistencia : IReporte<List<EventoConAsistenciaDto>>
{
    public string Nombre => "Eventos con mayor asistencia";

    public async Task<List<EventoConAsistenciaDto>> EjecutarAsync(FoodEventsDbContext dbContext, CancellationToken cancellationToken = default)
    {
        return await dbContext.EventosGastronomicos
            .Select(e => new EventoConAsistenciaDto(
                e.Id,
                e.Nombre,
                e.Reservas.Count(r => r.EstadoReserva == EstadoReserva.Confirmada)))
            .OrderByDescending(r => r.CantidadReservasConfirmadas)
            .ToListAsync(cancellationToken);
    }
}


