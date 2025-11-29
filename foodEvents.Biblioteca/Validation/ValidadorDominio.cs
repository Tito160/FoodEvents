using System.Net.Mail;

namespace FoodEvents.Biblioteca;

public class ValidadorDominio
{
    private ResultadoValidacion ValidarPersonaBase(PersonaBase persona, string nombreEntidad)
    {
        var resultado = new ResultadoValidacion();

        if (persona == null)
        {
            resultado.Errores.Add($"La instancia de {nombreEntidad} no puede ser nula.");
            return resultado;
        }

        if (string.IsNullOrWhiteSpace(persona.NombreCompleto))
        {
            resultado.Errores.Add($"El nombre completo de {nombreEntidad} es obligatorio.");
        }

        if (!EsEmailValido(persona.CorreoElectronico))
        {
            resultado.Errores.Add($"El correo electrónico de {nombreEntidad} no tiene un formato válido.");
        }

        if (!EsTelefonoValido(persona.Telefono))
        {
            resultado.Errores.Add($"El teléfono de {nombreEntidad} debe ser numérico y de longitud lógica.");
        }

        return resultado;
    }

    public ResultadoValidacion ValidarChef(Chef chef)
    {
        var resultado = ValidarPersonaBase(chef, "chef");

        if (string.IsNullOrWhiteSpace(chef.EspecialidadCulinaria))
        {
            resultado.Errores.Add("La especialidad culinaria del chef es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(chef.Nacionalidad))
        {
            resultado.Errores.Add("La nacionalidad del chef es obligatoria.");
        }

        if (chef.AniosExperiencia < 0)
        {
            resultado.Errores.Add("Los años de experiencia no pueden ser negativos.");
        }

        return resultado;
    }

    public ResultadoValidacion ValidarParticipante(Participante participante)
    {
        var resultado = ValidarPersonaBase(participante, "participante");

        if (string.IsNullOrWhiteSpace(participante.DocumentoIdentidad))
        {
            resultado.Errores.Add("El documento de identidad del participante es obligatorio.");
        }

        return resultado;
    }

    public ResultadoValidacion ValidarInvitadoEspecial(InvitadoEspecial invitado)
    {
        var resultado = ValidarPersonaBase(invitado, "invitado especial");

        // No hay reglas adicionales obligatorias por ahora, pero este método
        // permite especializar la validación si fuera necesario.

        return resultado;
    }

    public ResultadoValidacion ValidarEvento(EventoGastronomico evento)
    {
        var resultado = new ResultadoValidacion();

        if (evento == null)
        {
            resultado.Errores.Add("El evento no puede ser nulo.");
            return resultado;
        }

        if (string.IsNullOrWhiteSpace(evento.Nombre))
        {
            resultado.Errores.Add("El nombre del evento es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(evento.DescripcionDetallada))
        {
            resultado.Errores.Add("La descripción detallada del evento es obligatoria.");
        }

        if (evento.Modalidad == ModalidadEvento.Presencial)
        {
            if (string.IsNullOrWhiteSpace(evento.Ubicacion))
            {
                resultado.Errores.Add("La ubicación del evento presencial es obligatoria.");
            }
        }
        else if (evento.Modalidad == ModalidadEvento.Virtual)
        {
            if (string.IsNullOrWhiteSpace(evento.UrlAccesoVirtual))
            {
                resultado.Errores.Add("La URL de acceso del evento virtual es obligatoria.");
            }
        }

        if (evento.CapacidadMaxima <= 0)
        {
            resultado.Errores.Add("La capacidad máxima del evento debe ser mayor a cero.");
        }

        if (evento.PrecioPorEntrada < 0)
        {
            resultado.Errores.Add("El precio por entrada no puede ser negativo.");
        }

        if (evento.FechaFin < evento.FechaInicio)
        {
            resultado.Errores.Add("La fecha de fin no puede ser anterior a la fecha de inicio del evento.");
        }

        if (evento.ChefId <= 0)
        {
            resultado.Errores.Add("El evento debe estar asociado a un chef válido.");
        }

        return resultado;
    }

    public ResultadoValidacion ValidarReserva(Reserva reserva, EventoGastronomico? evento, int reservasConfirmadasActuales)
    {
        var resultado = new ResultadoValidacion();

        if (reserva == null)
        {
            resultado.Errores.Add("La reserva no puede ser nula.");
            return resultado;
        }

        if (evento == null)
        {
            resultado.Errores.Add("El evento asociado a la reserva no existe.");
            return resultado;
        }

        if (reserva.ParticipanteId <= 0)
        {
            resultado.Errores.Add("La reserva debe tener un participante válido.");
        }

        if (evento.CapacidadMaxima <= reservasConfirmadasActuales)
        {
            resultado.Errores.Add("No se puede reservar un lugar en un evento que ya está lleno.");
        }

        return resultado;
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
