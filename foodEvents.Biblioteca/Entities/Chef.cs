namespace FoodEvents.Biblioteca;

public class Chef
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string EspecialidadCulinaria { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public int AniosExperiencia { get; set; }
    public string CorreoElectronico { get; set; } = string.Empty;
    public string TelefonoContacto { get; set; } = string.Empty;

    public ICollection<EventoGastronomico> Eventos { get; set; } = new List<EventoGastronomico>();
}
