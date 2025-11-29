namespace FoodEvents.Biblioteca;

public class Reserva
{
    public int Id { get; set; }

    public int ParticipanteId { get; set; }
    public Participante? Participante { get; set; }

    public int EventoGastronomicoId { get; set; }
    public EventoGastronomico? Evento { get; set; }

    public DateTime FechaReserva { get; set; }
    public bool YaPago { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public EstadoReserva EstadoReserva { get; set; }
}
