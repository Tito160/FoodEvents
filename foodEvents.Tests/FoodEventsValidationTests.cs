using FoodEvents.Biblioteca;
using Xunit;

namespace foodEvents.Tests;

public class FoodEventsValidationTests
{
    private readonly ValidadorDominio _validador = new();

    [Fact]
    public void ChefConCorreoInvalido_DebeLanzarExcepcion()
    {
        var chef = new Chef
        {
            NombreCompleto = "Chef Prueba",
            EspecialidadCulinaria = "Cocina italiana",
            Nacionalidad = "Argentina",
            AniosExperiencia = 5,
            CorreoElectronico = "correo-invalido",
            TelefonoContacto = "1234567"
        };

        var excepcion = Assert.Throws<ValidacionDominioException>(() => _validador.ValidarChef(chef));

        Assert.NotEmpty(excepcion.Errores);
        Assert.Contains(excepcion.Errores, e => e.Contains("correo", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void EventoConFechaFinAntesDeInicio_DebeLanzarExcepcion()
    {
        var evento = new EventoGastronomico
        {
            Nombre = "Cena temática",
            DescripcionDetallada = "Cena temática de prueba",
            TipoEvento = TipoEventoGastronomico.CenaTematica,
            FechaInicio = new DateTime(2025, 1, 10),
            FechaFin = new DateTime(2025, 1, 9),
            CapacidadMaxima = 10,
            PrecioPorEntrada = 100,
            Ubicacion = "Buenos Aires",
            ChefId = 1
        };

        var excepcion = Assert.Throws<ValidacionDominioException>(() => _validador.ValidarEvento(evento));

        Assert.NotEmpty(excepcion.Errores);
        Assert.Contains(excepcion.Errores, e => e.Contains("fecha de fin", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ReservaEnEventoLleno_DebeLanzarExcepcion()
    {
        var evento = new EventoGastronomico
        {
            Nombre = "Cata de vinos",
            DescripcionDetallada = "Cata de prueba",
            TipoEvento = TipoEventoGastronomico.Cata,
            FechaInicio = DateTime.UtcNow,
            FechaFin = DateTime.UtcNow.AddHours(3),
            CapacidadMaxima = 1,
            PrecioPorEntrada = 50,
            Ubicacion = "Mendoza",
            ChefId = 1
        };

        var reserva = new Reserva
        {
            ParticipanteId = 1,
            EventoGastronomicoId = 1,
            EstadoReserva = EstadoReserva.Confirmada,
            MetodoPago = MetodoPago.Tarjeta,
            YaPago = true
        };

        var excepcion = Assert.Throws<ValidacionDominioException>(() => 
            _validador.ValidarReserva(reserva, evento, reservasConfirmadasActuales: 1));

        Assert.NotEmpty(excepcion.Errores);
        Assert.Contains(excepcion.Errores, e => e.Contains("evento que ya está lleno", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ChefValido_NoDebeLanzarExcepcion()
    {
        var chef = new Chef
        {
            NombreCompleto = "Gordon Ramsay",
            EspecialidadCulinaria = "Cocina francesa",
            Nacionalidad = "Reino Unido",
            AniosExperiencia = 20,
            CorreoElectronico = "gordon@example.com",
            TelefonoContacto = "1234567890"
        };

        // No debe lanzar excepción
        _validador.ValidarChef(chef);
    }

    [Fact]
    public void ParticipanteConDocumentoVacio_DebeLanzarExcepcion()
    {
        var participante = new Participante
        {
            NombreCompleto = "Juan Pérez",
            CorreoElectronico = "juan@example.com",
            Telefono = "1234567890",
            DocumentoIdentidad = ""
        };

        var excepcion = Assert.Throws<ValidacionDominioException>(() => 
            _validador.ValidarParticipante(participante));

        Assert.NotEmpty(excepcion.Errores);
        Assert.Contains(excepcion.Errores, e => e.Contains("documento", StringComparison.OrdinalIgnoreCase));
    }
}
