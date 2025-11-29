namespace FoodEvents.Biblioteca;

public class Chef : PersonaBase
{
    public string EspecialidadCulinaria { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public int AniosExperiencia { get; set; }

    // Alias para mantener compatibilidad semántica con código existente
    public string TelefonoContacto
    {
        get => Telefono;
        set => Telefono = value;
    }

    public ICollection<EventoGastronomico> Eventos { get; set; } = new List<EventoGastronomico>();

    public override string ObtenerResumenPerfil()
    {
        return $"{NombreCompleto} - Chef especializado en {EspecialidadCulinaria} ({AniosExperiencia} años de experiencia)";
    }
}
