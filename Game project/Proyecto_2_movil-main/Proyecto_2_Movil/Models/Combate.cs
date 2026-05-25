using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_2_Movil.Models
{
    public enum TurnoActual
    {
        Jugador1,
        Jugador2
    }

    public class Combate
    {
        public Jugador Jugador1 { get; set; } = null!;
        public Jugador Jugador2 { get; set; } = null!;

        // Vida actual de cada personaje
        public int VidaJ1 { get; set; } = 100;
        public int VidaJ2 { get; set; } = 100;

        // Distancia: 0 = cuerpo a cuerpo, >0 = lejos
        public int Distancia { get; set; } = 2;

        public TurnoActual Turno { get; set; } = TurnoActual.Jugador1;

        public bool CombateTerminado { get; set; }
        public string? Ganador { get; set; }
    }
}
