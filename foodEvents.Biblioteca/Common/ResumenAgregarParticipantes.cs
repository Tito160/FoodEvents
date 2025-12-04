using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodEvents.Biblioteca;

namespace foodEvents.Biblioteca.Common
{
    public class ResumenAgregarParticipantes
    {
        public int EventoId { get; set; }
        public int Confirmados { get; set; }
        public int EnEspera { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<Reserva> ReservasCreadas { get; set; } = new();
    }
}