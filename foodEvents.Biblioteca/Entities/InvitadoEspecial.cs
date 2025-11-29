namespace FoodEvents.Biblioteca;

public enum TipoInvitadoEspecial
{
    CriticoGastronomico = 0,
    Influencer = 1,
    FiguraReconocida = 2
}

/// <summary>
/// Representa a un invitado especial que participa de eventos sin necesidad de pagar,
/// pero que debe registrarse y confirmar asistencia.
/// </summary>
public class InvitadoEspecial : PersonaBase
{
    public TipoInvitadoEspecial TipoInvitado { get; set; }
    public string? DescripcionPerfilPublico { get; set; }

    public bool ConfirmoAsistencia { get; set; }

    public override string ObtenerResumenPerfil()
    {
        var tipo = TipoInvitado.ToString();
        var estado = ConfirmoAsistencia ? "Asistencia confirmada" : "Asistencia pendiente";
        return $"{NombreCompleto} - Invitado especial ({tipo}). {estado}.";
    }
}


