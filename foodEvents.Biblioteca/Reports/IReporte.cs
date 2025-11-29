namespace FoodEvents.Biblioteca.Reports;

/// <summary>
/// Contrato general para cualquier reporte del sistema.
/// </summary>
public interface IReporte<T>
{
    string Nombre { get; }

    Task<T> EjecutarAsync(FoodEventsDbContext dbContext, CancellationToken cancellationToken = default);
}


