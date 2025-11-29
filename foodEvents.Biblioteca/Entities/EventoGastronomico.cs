namespace FoodEvents.Biblioteca;

public class EventoGastronomico
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string DescripcionDetallada { get; set; } = string.Empty;
    public TipoEventoGastronomico TipoEvento { get; set; }
    public ModalidadEvento Modalidad { get; set; } = ModalidadEvento.Presencial;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int CapacidadMaxima { get; set; }
    public decimal PrecioPorEntrada { get; set; }
    public string Ubicacion { get; set; } = string.Empty;

    /// <summary>
    /// URL de acceso para eventos virtuales (clases online, catas por streaming, etc.).
    /// Solo aplica cuando la modalidad es virtual.
    /// </summary>
    public string? UrlAccesoVirtual { get; set; }

    public int ChefId { get; set; }
    public Chef? Chef { get; set; }

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
