// Services/PersistenciaService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Proyecto_2_Movil.Models;

namespace Proyecto_2_Movil.Services
{
    public class PersistenciaService
    {
        // ---------- Configuración de archivo JSON ----------
        private const string NombreArchivo = "datos_juego.json";
        private readonly string _rutaArchivo;
        private bool _inicializado;

        // ---------- Datos en memoria ----------
        // Los dos jugadores de la partida actual
        public List<Jugador> Jugadores { get; private set; } = new(2);

        // Historial de partidas (para estadísticas)
        public List<Partida> Historial { get; private set; } = new();

        // DTO para serializar/deserializar
        private class PersistenciaDto
        {
            public List<Jugador> Jugadores { get; set; } = new();
            public List<Partida> Historial { get; set; } = new();
        }

        public PersistenciaService()
        {
            _rutaArchivo = Path.Combine(FileSystem.AppDataDirectory, NombreArchivo);
        }

        // ==================================================
        // =      CARGA Y GUARDADO EN JSON (TU PARTE)       =
        // ==================================================

        /// <summary>
        /// Carga el archivo JSON si existe. Se llama una sola vez al inicio de la app.
        /// </summary>
        public async Task InicializarAsync()
        {
            if (_inicializado) return;

            if (File.Exists(_rutaArchivo))
            {
                try
                {
                    string json = await File.ReadAllTextAsync(_rutaArchivo);
                    var opciones = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var datos = JsonSerializer.Deserialize<PersistenciaDto>(json, opciones);
                    if (datos != null)
                    {
                        Jugadores = datos.Jugadores ?? new List<Jugador>();
                        Historial = datos.Historial ?? new List<Partida>();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error cargando JSON: {ex.Message}");
                    Jugadores = new List<Jugador>();
                    Historial = new List<Partida>();
                }
            }

            _inicializado = true;
        }

        /// <summary>
        /// Guarda jugadores + historial en el archivo JSON.
        /// </summary>
        private async Task GuardarAsync()
        {
            var dto = new PersistenciaDto
            {
                Jugadores = Jugadores,
                Historial = Historial
            };

            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(dto, opciones);
            await File.WriteAllTextAsync(_rutaArchivo, json);
        }

        /// <summary>
        /// Registra una partida terminada y actualiza estadísticas.
        /// </summary>
        public async Task RegistrarResultadoPartidaAsync(Jugador? ganador, Jugador? perdedor, bool esEmpate)
        {
            if (Jugadores.Count >= 2)
            {
                var j1 = Jugadores[0];
                var j2 = Jugadores[1];

                if (esEmpate)
                {
                    j1.Empates++;
                    j2.Empates++;
                }
                else if (ganador != null && perdedor != null)
                {
                    if (ganador.Nombre == j1.Nombre)
                    {
                        j1.PartidasGanadas++;
                        j2.PartidasPerdidas++;
                    }
                    else if (ganador.Nombre == j2.Nombre)
                    {
                        j2.PartidasGanadas++;
                        j1.PartidasPerdidas++;
                    }
                }
            }

            var partida = new Partida
            {
                Fecha = DateTime.Now,
                Jugador1 = Jugadores.Count > 0 ? Jugadores[0].Nombre : "",
                Jugador2 = Jugadores.Count > 1 ? Jugadores[1].Nombre : "",
                Ganador = esEmpate ? "" : ganador?.Nombre ?? "",
                EsEmpate = esEmpate
            };

            Historial.Add(partida);

            await GuardarAsync();
        }

        // ==================================================
        // =     MÉTODOS QUE YA USAN TUS COMPAÑEROS         =
        // ==================================================

        /// <summary>
        /// Se llama desde la pantalla de Registro de Jugadores.
        /// Crea los dos jugadores solo con el nombre.
        /// </summary>
        public void InicializarJugadores(string nombre1, string nombre2)
        {
            // Buscar si ya existen jugadores con esos nombres
            var j1 = Jugadores.FirstOrDefault(j => j.Nombre == nombre1);
            var j2 = Jugadores.FirstOrDefault(j => j.Nombre == nombre2);

            if (j1 == null)
            {
                j1 = new Jugador { Nombre = nombre1 };
                Jugadores.Add(j1);
            }

            if (j2 == null)
            {
                j2 = new Jugador { Nombre = nombre2 };
                Jugadores.Add(j2);
            }

            // Reset de Raza/Arma solo para la nueva partida
            j1.Raza = null;
            j1.Arma = null;

            j2.Raza = null;
            j2.Arma = null;

            // Asegurar que estos dos queden en las posiciones 0 y 1
            Jugadores.Remove(j1);
            Jugadores.Remove(j2);
            Jugadores.Insert(0, j1);
            Jugadores.Insert(1, j2);
        }


        // ------------ RAZAS ------------

        public Jugador? ObtenerJugadorSinRaza() =>
            Jugadores.FirstOrDefault(j => j.Raza == null);

        public bool TodosTienenRaza() =>
            Jugadores.All(j => j.Raza != null);

        // ------------ ARMAS ------------

        public Jugador? ObtenerJugadorSinArma() =>
            Jugadores.FirstOrDefault(j => j.Arma == null);

        public bool TodosTienenArma() =>
            Jugadores.All(j => j.Arma != null);

        public List<EstadisticaJugador> ObtenerEstadisticasJugadores()
        {
            return Jugadores
                .Select(j => new EstadisticaJugador
                {
                    Nombre = j.Nombre,
                    Ganadas = j.PartidasGanadas,
                    Perdidas = j.PartidasPerdidas,
                    Empates = j.Empates
                })
                .OrderByDescending(e => e.Ganadas)
                .ToList();
        }

    }
}
