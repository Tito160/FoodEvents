using FoodEvents.Biblioteca;

namespace foodEvents.WebApi.Dtos;

public static class MappingExtensions
{
    public static ChefResumenDto ToResumenDto(this Chef chef) => new()
    {
        Id = chef.Id,
        NombreCompleto = chef.NombreCompleto
    };

    public static EventoResumenDto ToResumenDto(this EventoGastronomico evento) => new()
    {
        Id = evento.Id,
        Nombre = evento.Nombre,
        TipoEvento = evento.TipoEvento,
        Modalidad = evento.Modalidad,
        FechaInicio = evento.FechaInicio,
        FechaFin = evento.FechaFin,
        Ubicacion = evento.Ubicacion
    };

    public static ParticipanteResumenDto ToResumenDto(this Participante participante) => new()
    {
        Id = participante.Id,
        NombreCompleto = participante.NombreCompleto
    };

    public static ReservaResumenDto ToResumenDto(this Reserva reserva) => new()
    {
        Id = reserva.Id,
        EstadoReserva = reserva.EstadoReserva,
        FechaReserva = reserva.FechaReserva,
        YaPago = reserva.YaPago
    };

    public static ChefDto ToDto(this Chef chef) => new()
    {
        Id = chef.Id,
        NombreCompleto = chef.NombreCompleto,
        EspecialidadCulinaria = chef.EspecialidadCulinaria,
        Nacionalidad = chef.Nacionalidad,
        AniosExperiencia = chef.AniosExperiencia,
        CorreoElectronico = chef.CorreoElectronico,
        TelefonoContacto = chef.TelefonoContacto,
        Eventos = chef.Eventos?.Select(e => e.ToResumenDto()).ToList() ?? new List<EventoResumenDto>()
    };

    public static EventoDto ToDto(this EventoGastronomico evento) => new()
    {
        Id = evento.Id,
        Nombre = evento.Nombre,
        DescripcionDetallada = evento.DescripcionDetallada,
        TipoEvento = evento.TipoEvento,
        Modalidad = evento.Modalidad,
        FechaInicio = evento.FechaInicio,
        FechaFin = evento.FechaFin,
        CapacidadMaxima = evento.CapacidadMaxima,
        PrecioPorEntrada = evento.PrecioPorEntrada,
        Ubicacion = evento.Ubicacion,
        UrlAccesoVirtual = evento.UrlAccesoVirtual,
        Chef = evento.Chef != null ? evento.Chef.ToResumenDto() : new ChefResumenDto { Id = evento.ChefId }
    };

    public static ParticipanteDto ToDto(this Participante participante) => new()
    {
        Id = participante.Id,
        NombreCompleto = participante.NombreCompleto,
        CorreoElectronico = participante.CorreoElectronico,
        Telefono = participante.Telefono,
        DocumentoIdentidad = participante.DocumentoIdentidad,
        RestriccionAlimentaria = participante.RestriccionAlimentaria,
        Reservas = participante.Reservas?.Select(r => r.ToResumenDto()).ToList() ?? new List<ReservaResumenDto>()
    };

    public static ReservaDto ToDto(this Reserva reserva) => new()
    {
        Id = reserva.Id,
        FechaReserva = reserva.FechaReserva,
        YaPago = reserva.YaPago,
        MetodoPago = reserva.MetodoPago,
        EstadoReserva = reserva.EstadoReserva,
        Participante = reserva.Participante != null
            ? reserva.Participante.ToResumenDto()
            : new ParticipanteResumenDto { Id = reserva.ParticipanteId },
        Evento = reserva.Evento != null
            ? reserva.Evento.ToResumenDto()
            : new EventoResumenDto { Id = reserva.EventoGastronomicoId }
    };
}
