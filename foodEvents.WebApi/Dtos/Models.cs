using FoodEvents.Biblioteca;

namespace foodEvents.WebApi.Dtos;

public class ChefResumenDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
}

public class EventoResumenDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TipoEventoGastronomico TipoEvento { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
}

public class ParticipanteResumenDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
}

public class ReservaResumenDto
{
    public int Id { get; set; }
    public EstadoReserva EstadoReserva { get; set; }
    public DateTime FechaReserva { get; set; }
    public bool YaPago { get; set; }
}

public class ChefDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string EspecialidadCulinaria { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public int AniosExperiencia { get; set; }
    public string CorreoElectronico { get; set; } = string.Empty;
    public string TelefonoContacto { get; set; } = string.Empty;

    // Para evitar loops, solo exponemos un resumen de los eventos sin incluir el chef dentro.
    public List<EventoResumenDto> Eventos { get; set; } = new();
}

public class EventoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string DescripcionDetallada { get; set; } = string.Empty;
    public TipoEventoGastronomico TipoEvento { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int CapacidadMaxima { get; set; }
    public decimal PrecioPorEntrada { get; set; }
    public string Ubicacion { get; set; } = string.Empty;

    public ChefResumenDto Chef { get; set; } = new();
}

public class ParticipanteDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public string? RestriccionAlimentaria { get; set; }

    public List<ReservaResumenDto> Reservas { get; set; } = new();
}

public class ReservaDto
{
    public int Id { get; set; }
    public DateTime FechaReserva { get; set; }
    public bool YaPago { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public EstadoReserva EstadoReserva { get; set; }

    public ParticipanteResumenDto Participante { get; set; } = new();
    public EventoResumenDto Evento { get; set; } = new();
}
