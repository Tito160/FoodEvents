namespace FoodEvents.Biblioteca;

public class Participante
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public string? RestriccionAlimentaria { get; set; }

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
