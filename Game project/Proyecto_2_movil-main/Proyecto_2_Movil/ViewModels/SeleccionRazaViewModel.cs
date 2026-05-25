// ViewModels/SeleccionRazaViewModel.cs
using System.Collections.ObjectModel;
using System.Windows.Input;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.Views;


namespace Proyecto_2_Movil.ViewModels
{
    public partial class SeleccionRazaViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;
        private Jugador? _jugadorActual;

        public SeleccionRazaViewModel(PersistenciaService servicio)
        {
            _servicio = servicio;
            Razas = new ObservableCollection<Raza>
            {
                new() { Nombre = "Humano", Descripcion = "Armas: Escopeta, Rifle. Sanar comiendo (~45%)." },
                new() { Nombre = "Elfo", Descripcion = "Báculo (Fuego, Tierra, Aire). Sanar ~65%." },
                new() { Nombre = "Elfo (Agua)", Descripcion = "Báculo de agua. +15 vida inicial. Sanar 75–90%." },
                new() { Nombre = "Orco", Descripcion = "Hacha (sangrado) o martillo. Sanar con poción." },
                new() { Nombre = "Bestia", Descripcion = "Puños (−10 vida) o espada. Sanar durmiendo (50%)." }
            };

            Actualizar();
            SeleccionarRazaCommand = new Command<Raza>(SeleccionarRaza);
        }

        public string Titulo => _jugadorActual != null
            ? $"Selecciona raza para {_jugadorActual.Nombre}"
            : "Error: no hay jugadores";

        public ObservableCollection<Raza> Razas { get; }
        public ICommand SeleccionarRazaCommand { get; }

        private void Actualizar()
        {
            _jugadorActual = _servicio.ObtenerJugadorSinRaza();
            OnPropertyChanged(nameof(Titulo));
        }

        private async void SeleccionarRaza(Raza raza)
        {
            if (_jugadorActual == null || raza == null) return;

            _jugadorActual.Raza = raza;
            _jugadorActual.VidaActual = raza.Nombre == "Elfo (Agua)" ? 115 : 100;

            if (_servicio.TodosTienenRaza())
            {
                await Shell.Current.GoToAsync(nameof(SeleccionArmaPage));

            }
            else
            {
                Actualizar(); // actualiza título sin navegar
            }
        }
    }
}