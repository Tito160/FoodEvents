namespace FoodEvents.Biblioteca;

public class Participante : PersonaBase
{
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public string? RestriccionAlimentaria { get; set; }

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public override string ObtenerResumenPerfil()
    {
        var restriccion = string.IsNullOrWhiteSpace(RestriccionAlimentaria)
            ? "Sin restricciones alimentarias"
            : $"Restricción alimentaria: {RestriccionAlimentaria}";

        return $"{NombreCompleto} - {restriccion}";
    }
}
