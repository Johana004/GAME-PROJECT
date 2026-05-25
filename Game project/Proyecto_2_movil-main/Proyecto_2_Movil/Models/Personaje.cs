using System;

namespace Proyecto_2_Movil.Models
{
    public class Personaje
    {
        public string Nombre { get; set; } = "";
        public string Raza { get; set; } = ""; // Usa string, igual que Jugador.Raza.Nombre
        public string Arma { get; set; } = ""; // Usa string, igual que Jugador.Arma.Nombre
        public int VidaActual { get; set; }
        public int VidaMaxima { get; set; }
        public int Distancia { get; private set; } = 3;
        public string Estado { get; private set; } = "Ninguno"; // "Sangrado", "Evasion", "Ninguno"
        public int TurnosRestantesEfecto { get; private set; } = 0;
        public string? ElementoMagico { get; set; } // "Fuego", "Tierra", "Aire", "Agua" o null

        private readonly Random _rnd = new();

        public Personaje(Jugador jugador)
        {
            Nombre = jugador.Nombre;
            VidaActual = jugador.VidaActual;
            VidaMaxima = VidaActual;

            // Copiamos directamente los valores de Jugador (sin enums)
            Raza = jugador.Raza?.Nombre ?? "Humano";
            Arma = jugador.Arma?.Nombre ?? "Espada";

            // Asignar elemento mágico solo para elfos
            if (Raza == "Elfo" || Raza == "Elfo (Agua)")
            {
                ElementoMagico = Raza == "Elfo (Agua)" ? "Agua" : "Fuego";
            }
        }

        public int CalcularDaño(bool esCuerpoACuerpo)
        {
            if (!esCuerpoACuerpo && Distancia > 0)
            {
                if (Raza == "Humano" && Arma.Contains("rifle", StringComparison.OrdinalIgnoreCase))
                    return _rnd.Next(10, 21);

                if (Raza == "Elfo" && ElementoMagico == "Aire")
                {
                    int baseDmg = _rnd.Next(1, 6);
                    return (int)(baseDmg * 1.15);
                }
                return 0;
            }

            // Ataque cuerpo a cuerpo
            if (Raza == "Humano")
            {
                return Arma.Contains("escopeta") || Arma.Contains("rifle")
                    ? _rnd.Next(1, 6) : _rnd.Next(1, 6);
            }

            if (Raza == "Elfo" || Raza == "Elfo (Agua)")
            {
                if (Arma.Contains("báculo") && !string.IsNullOrEmpty(ElementoMagico))
                {
                    return ElementoMagico switch
                    {
                        "Fuego" => _rnd.Next(3, 8),
                        "Tierra" => _rnd.Next(2, 7),
                        "Aire" => _rnd.Next(1, 6),
                        "Agua" => _rnd.Next(1, 6),
                        _ => _rnd.Next(1, 6)
                    };
                }
                return _rnd.Next(1, 6);
            }

            if (Raza == "Orco")
            {
                if (Arma.Contains("hacha"))
                {
                    Estado = "Sangrado";
                    TurnosRestantesEfecto = 2;
                    return _rnd.Next(1, 6);
                }
                if (Arma.Contains("martillo"))
                    return _rnd.Next(2, 8);
                return _rnd.Next(1, 6);
            }

            if (Raza == "Bestia")
            {
                if (Arma.Contains("puños"))
                {
                    VidaActual = Math.Max(0, VidaActual - 10);
                    return _rnd.Next(20, 31);
                }
                if (Arma.Contains("espada"))
                    return _rnd.Next(1, 11);
                return _rnd.Next(1, 6);
            }

            return _rnd.Next(1, 6);
        }

        public int Sanar()
        {
            int vidaPerdida = VidaMaxima - VidaActual;
            if (vidaPerdida <= 0) return 0;

            int curacion = 0;

            if (Raza == "Humano")
                curacion = (int)(vidaPerdida * 0.45);
            else if (Raza == "Elfo")
                curacion = (int)(vidaPerdida * 0.65);
            else if (Raza == "Elfo (Agua)")
            {
                double porc = 0.75 + (_rnd.NextDouble() * 0.15);
                curacion = (int)(vidaPerdida * porc);
            }
            else if (Raza == "Orco")
            {
                double basePct = 0.25 + (_rnd.NextDouble() * 0.20);
                if (TurnosRestantesEfecto > 0) basePct += 0.20;
                curacion = (int)(vidaPerdida * basePct);
            }
            else if (Raza == "Bestia")
                curacion = (int)(vidaPerdida * 0.50);
            else
                curacion = (int)(vidaPerdida * 0.40);

            VidaActual = Math.Min(VidaMaxima, VidaActual + curacion);
            return curacion;
        }

        public void Avanzar() => Distancia = Math.Max(0, Distancia - 1);
        public void Retroceder() => Distancia++;
        public void ActualizarEfectos()
        {
            if (TurnosRestantesEfecto > 0)
            {
                if (Estado == "Sangrado")
                    VidaActual = Math.Max(0, VidaActual - 3);
                TurnosRestantesEfecto--;
                if (TurnosRestantesEfecto == 0) Estado = "Ninguno";
            }
        }

        public string ObtenerImagenUri()
        {
            return Raza switch
            {
                "Humano" => "humano.jpeg",
                "Elfo" => "elfo.jpeg",
                "Elfo (Agua)" => "elfo_agua.jpeg",
                "Orco" => "orco.jpeg",
                "Bestia" => "bestia.jpeg",
                _ => "personaje_default.jpeg"
            };
        }
    }
}