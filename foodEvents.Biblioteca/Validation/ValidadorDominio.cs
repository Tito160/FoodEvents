using System.Net.Mail;

namespace FoodEvents.Biblioteca;

/// <summary>
/// Excepción personalizada para errores de validación de dominio.
/// </summary>
public class ValidacionDominioException : Exception
{
    public List<string> Errores { get; }

    public ValidacionDominioException(string mensaje, List<string> errores) : base(mensaje)
    {
        Errores = errores;
    }

    public ValidacionDominioException(string mensaje) : base(mensaje)
    {
        Errores = new List<string> { mensaje };
    }
}

public class ValidadorDominio
{
    private void ValidarPersonaBase(PersonaBase persona, string nombreEntidad)
    {
        var errores = new List<string>();

        if (persona == null)
        {
            throw new ValidacionDominioException($"La instancia de {nombreEntidad} no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(persona.NombreCompleto))
        {
            errores.Add($"El nombre completo de {nombreEntidad} es obligatorio.");
        }

        if (!EsEmailValido(persona.CorreoElectronico))
        {
            errores.Add($"El correo electrónico de {nombreEntidad} no tiene un formato válido.");
        }

        if (!EsTelefonoValido(persona.Telefono))
        {
            errores.Add($"El teléfono de {nombreEntidad} debe ser numérico y de longitud lógica.");
        }

        if (errores.Count > 0)
        {
            throw new ValidacionDominioException($"Errores de validación en {nombreEntidad}:", errores);
        }
    }

    public void ValidarChef(Chef chef)
    {
        var errores = new List<string>();

        ValidarPersonaBase(chef, "chef");

        if (string.IsNullOrWhiteSpace(chef.EspecialidadCulinaria))
        {
            errores.Add("La especialidad culinaria del chef es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(chef.Nacionalidad))
        {
            errores.Add("La nacionalidad del chef es obligatoria.");
        }

        if (chef.AniosExperiencia < 0)
        {
            errores.Add("Los años de experiencia no pueden ser negativos.");
        }

        if (errores.Count > 0)
        {
            throw new ValidacionDominioException("Errores de validación en chef:", errores);
        }
    }

    public void ValidarParticipante(Participante participante)
    {
        var errores = new List<string>();

        ValidarPersonaBase(participante, "participante");

        if (string.IsNullOrWhiteSpace(participante.DocumentoIdentidad))
        {
            errores.Add("El documento de identidad del participante es obligatorio.");
        }

        if (errores.Count > 0)
        {
            throw new ValidacionDominioException("Errores de validación en participante:", errores);
        }
    }

    public void ValidarInvitadoEspecial(InvitadoEspecial invitado)
    {
        ValidarPersonaBase(invitado, "invitado especial");

        // No hay reglas adicionales obligatorias por ahora, pero este método
        // permite especializar la validación si fuera necesario.
    }

    public void ValidarEvento(EventoGastronomico evento)
    {
        var errores = new List<string>();

        if (evento == null)
        {
            throw new ValidacionDominioException("El evento no puede ser nulo.");
        }

        if (string.IsNullOrWhiteSpace(evento.Nombre))
        {
            errores.Add("El nombre del evento es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(evento.DescripcionDetallada))
        {
            errores.Add("La descripción detallada del evento es obligatoria.");
        }

        if (evento.Modalidad == ModalidadEvento.Presencial)
        {
            if (string.IsNullOrWhiteSpace(evento.Ubicacion))
            {
                errores.Add("La ubicación del evento presencial es obligatoria.");
            }
        }
        else if (evento.Modalidad == ModalidadEvento.Virtual)
        {
            if (string.IsNullOrWhiteSpace(evento.UrlAccesoVirtual))
            {
                errores.Add("La URL de acceso del evento virtual es obligatoria.");
            }
        }

        if (evento.CapacidadMaxima <= 0)
        {
            errores.Add("La capacidad máxima del evento debe ser mayor a cero.");
        }

        if (evento.PrecioPorEntrada < 0)
        {
            errores.Add("El precio por entrada no puede ser negativo.");
        }

        if (evento.FechaFin < evento.FechaInicio)
        {
            errores.Add("La fecha de fin no puede ser anterior a la fecha de inicio del evento.");
        }

        if (evento.ChefId <= 0)
        {
            errores.Add("El evento debe estar asociado a un chef válido.");
        }

        if (errores.Count > 0)
        {
            throw new ValidacionDominioException("Errores de validación en evento:", errores);
        }
    }

    public void ValidarReserva(Reserva reserva, EventoGastronomico? evento, int reservasConfirmadasActuales)
    {
        var errores = new List<string>();

        if (reserva == null)
        {
            throw new ValidacionDominioException("La reserva no puede ser nula.");
        }

        if (evento == null)
        {
            throw new ValidacionDominioException("El evento asociado a la reserva no existe.");
        }

        if (reserva.ParticipanteId <= 0)
        {
            errores.Add("La reserva debe tener un participante válido.");
        }

        if (evento.CapacidadMaxima <= reservasConfirmadasActuales)
        {
            errores.Add("No se puede reservar un lugar en un evento que ya está lleno.");
        }

        if (errores.Count > 0)
        {
            throw new ValidacionDominioException("Errores de validación en reserva:", errores);
        }
    }

    private bool EsEmailValido(string? correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            return false;
        }

        try
        {
            var mail = new MailAddress(correo);
            return mail.Address == correo;
        }
        catch
        {
            return false;
        }
    }

    private bool EsTelefonoValido(string? telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
        {
            return false;
        }

        if (!telefono.All(char.IsDigit))
        {
            return false;
        }

        return telefono.Length is >= 7 and <= 15;
    }
}
