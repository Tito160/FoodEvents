namespace FoodEvents.Biblioteca;

/// <summary>
/// Representa el contrato mínimo para cualquier persona registrada en el sistema.
/// </summary>
public interface IPersonaRegistrable
{
    string NombreCompleto { get; }
    string CorreoElectronico { get; }
    string Telefono { get; }

    /// <summary>
    /// Lógica de registro dentro del sistema (puede ser un hook para auditar, etc.).
    /// </summary>
    void Registrar();

    /// <summary>
    /// Devuelve un resumen legible del perfil.
    /// </summary>
    string ObtenerResumenPerfil();

    /// <summary>
    /// Devuelve los datos de contacto formateados.
    /// </summary>
    string ObtenerDatosContacto();
}


