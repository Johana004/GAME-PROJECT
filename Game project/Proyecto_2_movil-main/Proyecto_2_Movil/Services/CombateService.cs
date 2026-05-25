using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Proyecto_2_Movil.Models;

namespace Proyecto_2_Movil.Services
{
    public class CombateService
    {
        private readonly Random _random = new();

        public Combate IniciarCombate(Jugador j1, Jugador j2)
        {
            return new Combate
            {
                Jugador1 = j1,
                Jugador2 = j2,
                VidaJ1 = 100,
                VidaJ2 = 100,
                Distancia = 2,
                Turno = TurnoActual.Jugador1
            };
        }

        public void Avanzar(Combate combate)
        {
            if (combate.Distancia > 0)
                combate.Distancia--;

            CambiarTurno(combate);
        }

        public void Retroceder(Combate combate)
        {
            combate.Distancia++;
            CambiarTurno(combate);
        }

        public string Atacar(Combate combate)
        {
            if (combate.CombateTerminado) return "La partida ya terminó.";

            // Ejemplo: solo se puede atacar cuerpo a cuerpo cuando Distancia == 0
            if (combate.Distancia > 0)
                return "Están muy lejos. Debes avanzar para atacar cuerpo a cuerpo.";

            int dano = _random.Next(5, 11); // daño simple 5–10

            if (combate.Turno == TurnoActual.Jugador1)
            {
                combate.VidaJ2 -= dano;
                if (combate.VidaJ2 <= 0)
                {
                    FinalizarCombate(combate, combate.Jugador1.Nombre);
                    return $"{combate.Jugador1.Nombre} hizo {dano} de daño y ganó la partida.";
                }

                CambiarTurno(combate);
                return $"{combate.Jugador1.Nombre} hizo {dano} de daño.";
            }
            else
            {
                combate.VidaJ1 -= dano;
                if (combate.VidaJ1 <= 0)
                {
                    FinalizarCombate(combate, combate.Jugador2.Nombre);
                    return $"{combate.Jugador2.Nombre} hizo {dano} de daño y ganó la partida.";
                }

                CambiarTurno(combate);
                return $"{combate.Jugador2.Nombre} hizo {dano} de daño.";
            }
        }

        public string Sanar(Combate combate)
        {
            if (combate.CombateTerminado) return "La partida ya terminó.";

            int curacion = _random.Next(8, 16); // curación simple

            if (combate.Turno == TurnoActual.Jugador1)
            {
                combate.VidaJ1 += curacion;
                if (combate.VidaJ1 > 100) combate.VidaJ1 = 100;
                CambiarTurno(combate);
                return $"{combate.Jugador1.Nombre} se curó {curacion} de vida.";
            }
            else
            {
                combate.VidaJ2 += curacion;
                if (combate.VidaJ2 > 100) combate.VidaJ2 = 100;
                CambiarTurno(combate);
                return $"{combate.Jugador2.Nombre} se curó {curacion} de vida.";
            }
        }

        private void CambiarTurno(Combate combate)
        {
            combate.Turno = combate.Turno == TurnoActual.Jugador1
                ? TurnoActual.Jugador2
                : TurnoActual.Jugador1;
        }

        private void FinalizarCombate(Combate combate, string ganador)
        {
            combate.CombateTerminado = true;
            combate.Ganador = ganador;
        }
    }
}
