using System;
using System.Windows.Input;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;

namespace Proyecto_2_Movil.ViewModels
{
    public class CombateViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;
        private readonly Random _random = new();

        public enum TurnoJugador
        {
            Jugador1,
            Jugador2
        }

        public CombateViewModel()
            : this(App.Current.Handler.MauiContext.Services.GetService<PersistenciaService>()!)
        {
        }

        public CombateViewModel(PersistenciaService servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));

            Jugador1 = _servicio.Jugadores[0];
            Jugador2 = _servicio.Jugadores[1];

            // Vida inicial según raza
            Jugador1.VidaActual = CalcularVidaInicial(Jugador1);
            Jugador2.VidaActual = CalcularVidaInicial(Jugador2);

            VidaJ1 = Jugador1.VidaActual;
            VidaJ2 = Jugador2.VidaActual;

            Distancia = 2;
            CombateTerminado = false;
            Ganador = string.Empty;

            Turno = TurnoJugador.Jugador1;
            TurnoTexto = $"Turno de {NombreTurnoActual}";
            Mensaje = "¡Empieza el combate!";

            AvanzarCommand = new Command(Avanzar);
            RetrocederCommand = new Command(Retroceder);
            AtacarCommand = new Command(Atacar);
            SanarCommand = new Command(Sanar);
            VerEstadisticasCommand = new Command(VerEstadisticas);
            OtraPartidaCommand = new Command(OtraPartida);
            SalirCommand = new Command(Salir);
        }


        // -------- Jugadores --------

        public Jugador Jugador1 { get; }
        public Jugador Jugador2 { get; }

        public string NombreJ1 => Jugador1.Nombre;
        public string NombreJ2 => Jugador2.Nombre;

        // -------- Estado del combate --------

        private TurnoJugador _turno;
        public TurnoJugador Turno
        {
            get => _turno;
            set => SetProperty(ref _turno, value);
        }

        private int _vidaJ1;
        public int VidaJ1
        {
            get => _vidaJ1;
            set => SetProperty(ref _vidaJ1, value);
        }

        private int _vidaJ2;
        public int VidaJ2
        {
            get => _vidaJ2;
            set => SetProperty(ref _vidaJ2, value);
        }

        private int _distancia;
        public int Distancia
        {
            get => _distancia;
            set => SetProperty(ref _distancia, value);
        }

        private bool _combateTerminado;
        public bool CombateTerminado
        {
            get => _combateTerminado;
            set => SetProperty(ref _combateTerminado, value);
        }

        private string _ganador = string.Empty;
        public string Ganador
        {
            get => _ganador;
            set => SetProperty(ref _ganador, value);
        }

        private string _turnoTexto = string.Empty;
        public string TurnoTexto
        {
            get => _turnoTexto;
            set => SetProperty(ref _turnoTexto, value);
        }

        private string _mensaje = string.Empty;
        public string Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        // -------- PROPIEDADES PARA NUEVA UI --------

        public double PorcentajeVidaJ1 => (double)VidaJ1 / ObtenerVidaMaxima(Jugador1);
        public double PorcentajeVidaJ2 => (double)VidaJ2 / ObtenerVidaMaxima(Jugador2);

        public bool EsTurnoJ1 => Turno == TurnoJugador.Jugador1;
        public bool EsTurnoJ2 => Turno == TurnoJugador.Jugador2;

        // Helpers internos
        private string NombreTurnoActual =>
            Turno == TurnoJugador.Jugador1 ? Jugador1.Nombre : Jugador2.Nombre;

        private string NombreTurnoOpuesto =>
            Turno == TurnoJugador.Jugador1 ? Jugador2.Nombre : Jugador1.Nombre;

        private Jugador JugadorEnTurno =>
            Turno == TurnoJugador.Jugador1 ? Jugador1 : Jugador2;

        private Jugador JugadorRival =>
            Turno == TurnoJugador.Jugador1 ? Jugador2 : Jugador1;

        // -------- Helpers de texto / coincidencias --------

        private static bool ContieneIgnoreCase(string? texto, string palabra)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            return texto.IndexOf(palabra, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // -------- Vida y curación por raza --------

        private bool EsHumano(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Humano");
        private bool EsElfo(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Elfo") && !ContieneIgnoreCase(j.Raza?.Nombre, "Agua");
        private bool EsElfoAgua(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Agua");
        private bool EsOrco(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Orco");
        private bool EsBestia(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Bestia");

        private int CalcularVidaInicial(Jugador j)
        {
            if (EsElfoAgua(j))
                return 115;
            return 100;
        }

        private int ObtenerVidaMaxima(Jugador j)
        {
            if (EsElfoAgua(j))
                return 130;
            return 100;
        }

        private int CalcularCuracion(Jugador j)
        {
            if (EsHumano(j)) return _random.Next(35, 51);
            if (EsElfo(j)) return _random.Next(55, 71);
            if (EsElfoAgua(j)) return _random.Next(75, 91);
            if (EsOrco(j)) return _random.Next(30, 46);
            if (EsBestia(j)) return _random.Next(40, 61);
            return _random.Next(25, 41);
        }

        // -------- Comandos --------

        public ICommand AvanzarCommand { get; }
        public ICommand RetrocederCommand { get; }
        public ICommand AtacarCommand { get; }
        public ICommand SanarCommand { get; }
        public ICommand VerEstadisticasCommand { get; }
        public ICommand OtraPartidaCommand { get; }
        public ICommand SalirCommand { get; }   



        // -------- Acciones --------

        private void Avanzar()
        {
            if (CombateTerminado) return;

            if (Distancia > 0)
            {
                Distancia--;
                Mensaje = $"{NombreTurnoActual} avanza.";
                CambiarTurno();
            }
            else
            {
                Mensaje = "Ya están cuerpo a cuerpo.";
            }

            NotificarCambios();
        }

        private void Retroceder()
        {
            if (CombateTerminado) return;

            Distancia++;
            Mensaje = $"{NombreTurnoActual} retrocede.";
            CambiarTurno();
            NotificarCambios();
        }

        private bool EstaEnRangoDeAtaque(Arma arma)
        {
            if (arma == null) return false;

            if (arma.EsDistancia)
                return Distancia <= 2;

            // Para armas cuerpo a cuerpo
            return Distancia == 0;
        }

        private void Atacar()
        {
            if (CombateTerminado) return;

            var arma = JugadorEnTurno.Arma;
            if (arma == null)
            {
                Mensaje = "No tienes arma equipada.";
                return;
            }

            if (!EstaEnRangoDeAtaque(arma))
            {
                Mensaje = arma.EsDistancia
                    ? "Necesitas acercarte un poco más para usar esa arma."
                    : "Están demasiado lejos para atacar cuerpo a cuerpo.";
                return;
            }

            // Se delega a método que calcula según arma
            EjecutarAtaqueConArma(arma);
            NotificarCambios();
        }

        private void EjecutarAtaqueConArma(Arma arma)
        {
            if (_random.NextDouble() < 0.02)
            {
                // pequeña probabilidad de fallo global (opcional); mantén o quita
            }

            // Decide baseMin/baseMax según arma
            int danoGenerado = 0;
            int totalDano = 0;
            int criticos = 0;
            int golpes = arma.Nombre.Contains("Dagas", StringComparison.OrdinalIgnoreCase) ? 2 : 1;

            for (int g = 0; g < golpes; g++)
            {
                int dano = 0;

                // ********* Lógica por arma *********
                if (arma.Nombre == "Escopeta")
                {
                    // daño 1-5 con pequeño extra aleatorio
                    dano = _random.Next(1, 6) + _random.Next(0, 2); // 1-6
                }
                else if (arma.Nombre == "Rifle francotirador")
                {
                    if (Distancia == 2)
                    {
                        dano = _random.Next(10, 21); // 10-20 en distancia larga
                    }
                    else
                    {
                        dano = _random.Next(1, 6); // 1-5 en cercanía
                    }
                }
                else if (arma.EsMagico && arma.Elemento != null)
                {
                    // Báculos: variar por elemento
                    switch (arma.Elemento)
                    {
                        case "Fuego":
                            dano = _random.Next(3, 9); // más daño porcentual
                            break;
                        case "Tierra":
                            dano = _random.Next(2, 8); // ligero bono
                            break;
                        case "Aire":
                            dano = _random.Next(1, 6); // daño base normal, posibilidad de evasión adicional manejada en evasión
                            break;
                        case "Agua":
                            dano = _random.Next(1, 6); // daño estándar, vida y sanación superior cubiertos por raza
                            break;
                        default:
                            dano = _random.Next(1, 6);
                            break;
                    }
                }
                else if (arma.Nombre == "Hacha")
                {
                    dano = _random.Next(1, 6);
                }
                else if (arma.Nombre == "Martillo")
                {
                    dano = _random.Next(2, 8);
                }
                else if (arma.Nombre == "Puños")
                {
                    // Bestia: daño muy alto y el atacante pierde vida
                    dano = _random.Next(20, 31);
                }
                else if (arma.Nombre == "Espada")
                {
                    dano = _random.Next(10, 17);
                }
                else if (arma.Nombre == "Dagas")
                {
                    dano = _random.Next(7, 12);
                }
                else
                {
                    // fallback
                    dano = arma.AtaqueBase > 0 ? arma.AtaqueBase : _random.Next(1, 8);
                }

                // Bonus por distancia definido en el arma
                if (arma.BonusDistanciaPercent > 0 && arma.EsDistancia && Distancia == 2)
                {
                    dano = (int)Math.Round(dano * (1 + arma.BonusDistanciaPercent / 100.0));
                }

                // Calcular evasión del defensor
                var defensor = JugadorRival;
                double probEvasion = defensor?.ProbabilidadEvasion ?? 0.0;

                // Si el defensor tiene elemento "Aire" como raza/arma noches adicionales ya asignadas en ProbabilidadEvasion
                if (_random.NextDouble() < probEvasion)
                {
                    // Evadió el golpe (si hay doble golpe, el siguiente golpe aún puede intentar)
                    Mensaje = $"¡{defensor?.Nombre} evadió el ataque!";
                    continue;
                }

                // Chance de crítico (usamos una base simple)
                double critChance = 0.15;
                if (_random.NextDouble() < critChance)
                {
                    dano = (int)Math.Round(dano * 1.5);
                    criticos++;
                }

                // Si es el arma Puños, atacante pierde vida
                if (arma.Nombre == "Puños")
                {
                    if (Turno == TurnoJugador.Jugador1)
                    {
                        VidaJ1 = Math.Max(0, VidaJ1 - 10); // pierde 10 al atacar
                    }
                    else
                    {
                        VidaJ2 = Math.Max(0, VidaJ2 - 10);
                    }
                }

                totalDano += dano;

                // Aplicar daño
                if (Turno == TurnoJugador.Jugador1)
                {
                    VidaJ2 -= dano;
                    if (VidaJ2 <= 0)
                    {
                        VidaJ2 = 0;
                        SincronizarVidaConJugadores();
                        Mensaje = $"{Jugador1.Nombre} atacó con {arma.Nombre} e hizo {totalDano} de daño (críticos: {criticos}) y ganó.";
                        FinalizarCombate(Jugador1.Nombre);
                        return;
                    }
                    else
                    {
                        // Aplicar sangrado si arma lo causa
                        if (arma.CausaSangrado)
                        {
                            AplicarSangradoEnDefensor(Jugador2);
                        }
                    }
                }
                else
                {
                    VidaJ1 -= dano;
                    if (VidaJ1 <= 0)
                    {
                        VidaJ1 = 0;
                        SincronizarVidaConJugadores();
                        Mensaje = $"{Jugador2.Nombre} atacó con {arma.Nombre} e hizo {totalDano} de daño (críticos: {criticos}) y ganó.";
                        FinalizarCombate(Jugador2.Nombre);
                        return;
                    }
                    else
                    {
                        if (arma.CausaSangrado)
                        {
                            AplicarSangradoEnDefensor(Jugador1);
                        }
                    }
                }
            } // fin ciclo golpes

            Mensaje = $"{NombreTurnoActual} atacó con {arma.Nombre} e hizo {totalDano} de daño (críticos: {criticos}).";
            CambiarTurno();
        }

        private void AplicarSangradoEnDefensor(Jugador defensor)
        {
            defensor.TieneSangrado = true;
            defensor.TurnosSangrado = 2;
            defensor.DanioSangrado = _random.Next(5, 11); // 5-10
        }

        private async void Salir()
        {
            if (!CombateTerminado)
                return;

            // Regresa a la primera pantalla del flujo
            await Shell.Current.GoToAsync("//InicioPage");
        }


        private void Sanar()
        {
            if (CombateTerminado) return;

            int curacion = CalcularCuracion(JugadorEnTurno);

            if (Turno == TurnoJugador.Jugador1)
            {
                VidaJ1 = Math.Min(VidaJ1 + curacion, ObtenerVidaMaxima(Jugador1));
            }
            else
            {
                VidaJ2 = Math.Min(VidaJ2 + curacion, ObtenerVidaMaxima(Jugador2));
            }

            Mensaje = $"{NombreTurnoActual} recuperó {curacion} de vida.";
            CambiarTurno();
            NotificarCambios();
        }

        // -------- Helpers internos --------

        private void CambiarTurno()
        {
            if (CombateTerminado) return;

            Turno = Turno == TurnoJugador.Jugador1
                ? TurnoJugador.Jugador2
                : TurnoJugador.Jugador1;

            TurnoTexto = $"Turno de {NombreTurnoActual}";

            // Aplicar efectos al inicio del turno del jugador que ahora tiene el turno
            AplicarEfectosPorTurno(JugadorEnTurno);

            NotificarCambios();
        }

        private void AplicarEfectosPorTurno(Jugador jugadorEnTurno)
        {
            if (jugadorEnTurno == null) return;

            // Sangrado
            if (jugadorEnTurno.TieneSangrado && jugadorEnTurno.TurnosSangrado > 0)
            {
                int dmg = jugadorEnTurno.DanioSangrado;
                if (jugadorEnTurno == Jugador1)
                    VidaJ1 = Math.Max(0, VidaJ1 - dmg);
                else
                    VidaJ2 = Math.Max(0, VidaJ2 - dmg);

                jugadorEnTurno.TurnosSangrado--;
                Mensaje = $"{jugadorEnTurno.Nombre} sufre {dmg} de daño por sangrado. ({jugadorEnTurno.TurnosSangrado} turnos restantes)";

                if (jugadorEnTurno.TurnosSangrado <= 0)
                {
                    jugadorEnTurno.TieneSangrado = false;
                    jugadorEnTurno.DanioSangrado = 0;
                }

                // Verificar muerte por sangrado
                if (VidaJ1 <= 0)
                {
                    VidaJ1 = 0;
                    SincronizarVidaConJugadores();
                    FinalizarCombate(Jugador2.Nombre);
                    Mensaje += $" {Jugador2.Nombre} gana por muerte por sangrado.";
                    return;
                }

                if (VidaJ2 <= 0)
                {
                    VidaJ2 = 0;
                    SincronizarVidaConJugadores();
                    FinalizarCombate(Jugador1.Nombre);
                    Mensaje += $" {Jugador1.Nombre} gana por muerte por sangrado.";
                    return;
                }
            }

            SincronizarVidaConJugadores();
        }

        private void FinalizarCombate(string nombreGanador)
        {
            CombateTerminado = true;
            Ganador = nombreGanador;
            TurnoTexto = $"Ganador: {nombreGanador}";

            // Determinar quién ganó y quién perdió
            Jugador ganador = Jugador1;
            Jugador perdedor = Jugador2;

            if (nombreGanador == Jugador2.Nombre)
            {
                ganador = Jugador2;
                perdedor = Jugador1;
            }

            // Guardar estadísticas + historial en segundo plano
            _ = _servicio.RegistrarResultadoPartidaAsync(ganador, perdedor, esEmpate: false);
        }


        private void SincronizarVidaConJugadores()
        {
            Jugador1.VidaActual = VidaJ1;
            Jugador2.VidaActual = VidaJ2;
        }
        private async void VerEstadisticas()
        {
            // Solo tiene sentido ir a estadísticas si el combate ya terminó
            if (!CombateTerminado)
                return;

            // Navega a la página de estadísticas (ruta registrada en AppShell)
            await Shell.Current.GoToAsync("EstadisticasPage");
        }

        private void OtraPartida()
        {
            if (!CombateTerminado)
                return;

            ReiniciarCombate();
        }

        private void ReiniciarCombate()
        {
            // Reset de estado de combate
            CombateTerminado = false;
            Ganador = string.Empty;

            // Reset de sangrado / efectos si tu modelo los tiene
            Jugador1.TieneSangrado = false;
            Jugador1.TurnosSangrado = 0;
            Jugador1.DanioSangrado = 0;

            Jugador2.TieneSangrado = false;
            Jugador2.TurnosSangrado = 0;
            Jugador2.DanioSangrado = 0;

            // Vida inicial de nuevo según raza
            VidaJ1 = CalcularVidaInicial(Jugador1);
            VidaJ2 = CalcularVidaInicial(Jugador2);

            // Sincronizar con el modelo
            SincronizarVidaConJugadores();

            // Distancia inicial
            Distancia = 2;

            // Turno vuelve a jugador 1
            Turno = TurnoJugador.Jugador1;
            TurnoTexto = $"Turno de {NombreTurnoActual}";

            Mensaje = "Nueva partida iniciada con los mismos personajes.";

            NotificarCambios();
        }

        private void NotificarCambios()
        {
            OnPropertyChanged(nameof(VidaJ1));
            OnPropertyChanged(nameof(VidaJ2));
            OnPropertyChanged(nameof(PorcentajeVidaJ1));
            OnPropertyChanged(nameof(PorcentajeVidaJ2));
            OnPropertyChanged(nameof(EsTurnoJ1));
            OnPropertyChanged(nameof(EsTurnoJ2));
            OnPropertyChanged(nameof(Distancia));
            OnPropertyChanged(nameof(Turno));
            OnPropertyChanged(nameof(TurnoTexto));
            OnPropertyChanged(nameof(Mensaje));
        }
    }
}