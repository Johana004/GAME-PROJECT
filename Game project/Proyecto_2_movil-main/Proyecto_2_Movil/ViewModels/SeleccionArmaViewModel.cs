using System.Collections.ObjectModel;
using System.Windows.Input;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.Views;
using System.Linq;

namespace Proyecto_2_Movil.ViewModels
{
    public partial class SeleccionArmaViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;
        private Jugador? _jugadorActual;

        // Lista maestra con todas las armas del PDF
        private readonly List<Arma> _armasMaestras = new()
        {
            // Humanos -> armas de fuego: Escopeta, Rifle
            new() { Nombre="Escopeta", Descripcion="Daño corto rango (1-5) con pequeño extra.", AtaqueBase=5, Tipo="Distancia", EsDistancia=true, CausaSangrado=false, EsMagico=false, BonusDistanciaPercent=0 },
            new() { Nombre="Rifle francotirador", Descripcion="Daño a distancia. Mayor daño en máxima distancia.", AtaqueBase=5, Tipo="Distancia", EsDistancia=true, CausaSangrado=false, EsMagico=false, BonusDistanciaPercent=100 },

            // Elfos -> báculo (varios elementos)
            new() { Nombre="Báculo (Fuego)", Descripcion="Báculo elemental: fuego (más daño porcentual).", AtaqueBase=6, Tipo="Magia", EsDistancia=false, CausaSangrado=false, EsMagico=true, Elemento="Fuego" },
            new() { Nombre="Báculo (Tierra)", Descripcion="Báculo elemental: tierra (bono de ataque y mayor probabilidad de acierto).", AtaqueBase=5, Tipo="Magia", EsDistancia=false, CausaSangrado=false, EsMagico=true, Elemento="Tierra" },
            new() { Nombre="Báculo (Aire)", Descripcion="Báculo elemental: aire (posibilidad de evasión/velocidad).", AtaqueBase=5, Tipo="Magia", EsDistancia=false, CausaSangrado=false, EsMagico=true, Elemento="Aire" },
            new() { Nombre="Báculo (Agua)", Descripcion="Báculo de agua (mejor sanación y vida inicial).", AtaqueBase=4, Tipo="Magia", EsDistancia=false, CausaSangrado=false, EsMagico=true, Elemento="Agua" },

            // Orcos -> Hacha o Martillo
            new() { Nombre="Hacha", Descripcion="Hacha: daño base 1-5 y aplica sangrado por 2 turnos.", AtaqueBase=6, Tipo="Cuerpo", EsDistancia=false, CausaSangrado=true, EsMagico=false },
            new() { Nombre="Martillo", Descripcion="Martillo: daño base 2-7.", AtaqueBase=8, Tipo="Cuerpo", EsDistancia=false, CausaSangrado=false, EsMagico=false },

            // Bestias -> Puños o Espada
            new() { Nombre="Puños", Descripcion="Puños: ataque físico brutal (20-30) pero el atacante pierde 10 vida.", AtaqueBase=25, Tipo="Cuerpo", EsDistancia=false, CausaSangrado=false, EsMagico=false, PierdeVidaAlAtacar=true },
            new() { Nombre="Espada", Descripcion="Espada: daño equilibrado.", AtaqueBase=10, Tipo="Cuerpo", EsDistancia=false, CausaSangrado=false, EsMagico=false },

            // Dagas añadidas como opción (puede usarse por algunos)
            new() { Nombre="Dagas", Descripcion="Dagas: golpes dobles y aplican sangrado ocasional.", AtaqueBase=7, Tipo="Cuerpo", EsDistancia=false, CausaSangrado=true, EsMagico=false }
        };

        public ObservableCollection<Arma> Armas { get; }
        public ICommand SeleccionarArmaCommand { get; }

        public string Titulo => _jugadorActual != null
            ? $"Selecciona arma para {_jugadorActual.Nombre}"
            : "Error: no hay jugadores";

        public SeleccionArmaViewModel()
            : this(App.Current.Handler.MauiContext.Services.GetService<PersistenciaService>())
        {
        }

        public SeleccionArmaViewModel(PersistenciaService servicio)
        {
            _servicio = servicio;
            Armas = new ObservableCollection<Arma>();
            SeleccionarArmaCommand = new Command<Arma>(SeleccionarArma);
            Actualizar();
        }

        private void Actualizar()
        {
            _jugadorActual = _servicio.ObtenerJugadorSinArma();
            OnPropertyChanged(nameof(Titulo));
            FiltrarArmasSegunRaza();
        }

        private void FiltrarArmasSegunRaza()
        {
            Armas.Clear();

            if (_jugadorActual == null || _jugadorActual.Raza == null)
            {
                // Si no hay raza, mostramos todo para evitar bloquear al usuario.
                foreach (var a in _armasMaestras)
                    Armas.Add(a);
                return;
            }

            var raza = _jugadorActual.Raza.Nombre;

            IEnumerable<Arma> permitidas = raza switch
            {
                "Humano" => _armasMaestras.Where(a => a.Nombre == "Escopeta" || a.Nombre == "Rifle francotirador"),
                "Elfo" => _armasMaestras.Where(a => a.EsMagico && (a.Elemento == "Fuego" || a.Elemento == "Tierra" || a.Elemento == "Aire")),
                "Elfo (Agua)" => _armasMaestras.Where(a => a.EsMagico && a.Elemento == "Agua"),
                "Orco" => _armasMaestras.Where(a => a.Nombre == "Hacha" || a.Nombre == "Martillo"),
                "Bestia" => _armasMaestras.Where(a => a.Nombre == "Puños" || a.Nombre == "Espada"),
                _ => _armasMaestras
            };

            foreach (var a in permitidas)
                Armas.Add(a);
        }

        private async void SeleccionarArma(Arma arma)
        {
            if (_jugadorActual == null || arma == null)
                return;

            _jugadorActual.Arma = arma;

            _jugadorActual.ProbabilidadEvasion = 0.0;

            if (_jugadorActual.Raza != null)
            {
                var rName = _jugadorActual.Raza.Nombre;
                if (rName.Contains("Elfo"))
                    _jugadorActual.ProbabilidadEvasion += 0.15;
                else if (rName.Contains("Bestia"))
                    _jugadorActual.ProbabilidadEvasion += 0.10;
            }

            if (arma.Nombre.Contains("Dagas"))
                _jugadorActual.ProbabilidadEvasion += 0.10;

            if (arma.EsMagico && arma.Elemento == "Aire")
                _jugadorActual.ProbabilidadEvasion += 0.05;

            // ✅ Commit 15: persistencia correcta del personaje
            var personaje = new Personaje(_jugadorActual);
            CharacterPreferencesService.Guardar(personaje);

            if (_servicio.TodosTienenArma())
            {
                await Shell.Current.GoToAsync(nameof(CombatePage));
            }
            else
            {
                Actualizar();
            }
        }


    }
}
