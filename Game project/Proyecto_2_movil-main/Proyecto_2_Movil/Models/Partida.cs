using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_2_Movil.Models
{
    public class Partida
    {
        public DateTime Fecha { get; set; } = DateTime.Now;

        public string Jugador1 { get; set; } = string.Empty;
        public string Jugador2 { get; set; } = string.Empty;

        // Nombre del ganador, o vacío si fue empate
        public string Ganador { get; set; } = string.Empty;

        public bool EsEmpate { get; set; }
    }
}
