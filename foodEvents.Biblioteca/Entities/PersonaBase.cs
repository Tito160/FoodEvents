namespace FoodEvents.Biblioteca;

/// <summary>
/// Clase base abstracta que concentra el comportamiento y datos comunes
/// de todas las personas que interactúan con el sistema.
/// </summary>
public abstract class PersonaBase : IPersonaRegistrable
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    public virtual void Registrar()
    {
        // En una implementación real podría registrar auditoría, fecha de alta, etc.
        // Aquí dejamos una implementación base vacía que las subclases pueden sobreescribir.
    }

    public virtual string ObtenerResumenPerfil()
    {
        return $"{NombreCompleto} - {CorreoElectronico}";
    }

    public virtual string ObtenerDatosContacto()
    {
        return $"{NombreCompleto} | Email: {CorreoElectronico} | Teléfono: {Telefono}";
    }
}


