using Microsoft.EntityFrameworkCore;

namespace FoodEvents.Biblioteca;

public class FoodEventsService
{
    private readonly FoodEventsDbContext _dbContext;
    private readonly ValidadorDominio _validador;

    public FoodEventsService(FoodEventsDbContext dbContext, ValidadorDominio validador)
    {
        _dbContext = dbContext;
        _validador = validador;
    }

    public Task<List<EventoGastronomico>> ObtenerEventosAsync()
    {
        return _dbContext.EventosGastronomicos
            .Include(e => e.Chef)
            .Include(e => e.Reservas)
            .ToListAsync();
    }

    public Task<EventoGastronomico?> ObtenerEventoPorIdAsync(int id)
    {
        return _dbContext.EventosGastronomicos
            .Include(e => e.Chef)
            .Include(e => e.Reservas)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<List<Chef>> ObtenerChefsAsync()
    {
        return _dbContext.Chefs
            .Include(c => c.Eventos)
            .ToListAsync();
    }

    public Task<Chef?> ObtenerChefPorIdAsync(int id)
    {
        return _dbContext.Chefs
            .Include(c => c.Eventos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Participante>> ObtenerParticipantesAsync()
    {
        return _dbContext.Participantes
            .Include(p => p.Reservas)
            .ToListAsync();
    }

    public Task<Participante?> ObtenerParticipantePorIdAsync(int id)
    {
        return _dbContext.Participantes
            .Include(p => p.Reservas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<InvitadoEspecial>> ObtenerInvitadosEspecialesAsync()
    {
        return _dbContext.InvitadosEspeciales.ToListAsync();
    }

    public Task<InvitadoEspecial?> ObtenerInvitadoEspecialPorIdAsync(int id)
    {
        return _dbContext.InvitadosEspeciales.FirstOrDefaultAsync(i => i.Id == id);
    }

    public Task<List<Reserva>> ObtenerReservasAsync()
    {
        return _dbContext.Reservas
            .Include(r => r.Evento)!
            .ThenInclude(e => e!.Chef)
            .Include(r => r.Participante)
            .ToListAsync();
    }

    public Task<Reserva?> ObtenerReservaPorIdAsync(int id)
    {
        return _dbContext.Reservas
            .Include(r => r.Evento)!
            .ThenInclude(e => e!.Chef)
            .Include(r => r.Participante)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<ResultadoOperacion<Chef>> CrearChefAsync(Chef chef)
    {
        var resultado = new ResultadoOperacion<Chef>();

        try
        {
            _validador.ValidarChef(chef);

            _dbContext.Chefs.Add(chef);
            await _dbContext.SaveChangesAsync();

            resultado.Valor = chef;
        }
        catch (ValidacionDominioException ex)
        {
            resultado.Errores.AddRange(ex.Errores);
        }
        catch (Exception ex)
        {
            resultado.Errores.Add($"Error al guardar el chef: {ex.Message}");
        }

        return resultado;
    }

    public async Task<ResultadoOperacion<Participante>> CrearParticipanteAsync(Participante participante)
    {
        var resultado = new ResultadoOperacion<Participante>();

        try
        {
            _validador.ValidarParticipante(participante);

            _dbContext.Participantes.Add(participante);
            await _dbContext.SaveChangesAsync();

            resultado.Valor = participante;
        }
        catch (ValidacionDominioException ex)
        {
            resultado.Errores.AddRange(ex.Errores);
        }
        catch (Exception ex)
        {
            resultado.Errores.Add($"Error al guardar el participante: {ex.Message}");
        }

        return resultado;
    }

    public async Task<ResultadoOperacion<InvitadoEspecial>> CrearInvitadoEspecialAsync(InvitadoEspecial invitado)
    {
        var resultado = new ResultadoOperacion<InvitadoEspecial>();

        try
        {
            _validador.ValidarInvitadoEspecial(invitado);

            _dbContext.InvitadosEspeciales.Add(invitado);
            await _dbContext.SaveChangesAsync();

            resultado.Valor = invitado;
        }
        catch (ValidacionDominioException ex)
        {
            resultado.Errores.AddRange(ex.Errores);
        }
        catch (Exception ex)
        {
            resultado.Errores.Add($"Error al guardar el invitado especial: {ex.Message}");
        }

        return resultado;
    }

    public async Task<ResultadoOperacion<EventoGastronomico>> CrearEventoAsync(EventoGastronomico evento)
    {
        var resultado = new ResultadoOperacion<EventoGastronomico>();

        try
        {
            _validador.ValidarEvento(evento);

            var chefExiste = await _dbContext.Chefs.AnyAsync(c => c.Id == evento.ChefId);
            if (!chefExiste)
            {
                resultado.Errores.Add("El chef especificado no existe.");
                return resultado;
            }

            _dbContext.EventosGastronomicos.Add(evento);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(evento).Reference(e => e.Chef).LoadAsync();

            resultado.Valor = evento;
        }
        catch (ValidacionDominioException ex)
        {
            resultado.Errores.AddRange(ex.Errores);
        }
        catch (Exception ex)
        {
            resultado.Errores.Add($"Error al guardar el evento: {ex.Message}");
        }

        return resultado;
    }

    public async Task<bool> EliminarEventoAsync(int id)
    {
        var evento = await _dbContext.EventosGastronomicos.FindAsync(id);
        if (evento is null)
        {
            return false;
        }

        _dbContext.EventosGastronomicos.Remove(evento);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<ResultadoOperacion<Reserva>> CrearReservaAsync(Reserva reserva)
    {
        var resultado = new ResultadoOperacion<Reserva>();

        try
        {
            var evento = await _dbContext.EventosGastronomicos
                .Include(e => e.Reservas)
                .FirstOrDefaultAsync(e => e.Id == reserva.EventoGastronomicoId);

            if (evento is null)
            {
                resultado.Errores.Add("El evento especificado no existe.");
                return resultado;
            }

            var participante = await _dbContext.Participantes.FindAsync(reserva.ParticipanteId);
            if (participante is null)
            {
                resultado.Errores.Add("El participante especificado no existe.");
                return resultado;
            }

            var reservasConfirmadas = evento.Reservas.Count(r => r.EstadoReserva == EstadoReserva.Confirmada);

            _validador.ValidarReserva(reserva, evento, reservasConfirmadas);

            reserva.FechaReserva = DateTime.UtcNow;

            _dbContext.Reservas.Add(reserva);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(reserva).Reference(r => r.Evento).LoadAsync();
            await _dbContext.Entry(reserva).Reference(r => r.Participante).LoadAsync();

            resultado.Valor = reserva;
        }
        catch (ValidacionDominioException ex)
        {
            resultado.Errores.AddRange(ex.Errores);
        }
        catch (Exception ex)
        {
            resultado.Errores.Add($"Error al guardar la reserva: {ex.Message}");
        }

        return resultado;
    }

    public async Task<bool> CancelarReservaAsync(int reservaId)
    {
        var reserva = await _dbContext.Reservas.FindAsync(reservaId);
        if (reserva is null)
        {
            return false;
        }

        reserva.EstadoReserva = EstadoReserva.Cancelada;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
