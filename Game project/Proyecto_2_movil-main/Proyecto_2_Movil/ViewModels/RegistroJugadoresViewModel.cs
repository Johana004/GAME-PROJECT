// ViewModels/RegistroJugadoresViewModel.cs
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Views;


namespace Proyecto_2_Movil.ViewModels
{
    public partial class RegistroJugadoresViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;

        public RegistroJugadoresViewModel(PersistenciaService servicio)
        {
            _servicio = servicio;
            ContinuarCommand = new Command(async () => await ContinuarAsync());
        }

        private string _nombre1 = "";
        public string NombreJugador1
        {
            get => _nombre1;
            set => SetProperty(ref _nombre1, value);
        }

        private string _nombre2 = "";
        public string NombreJugador2
        {
            get => _nombre2;
            set => SetProperty(ref _nombre2, value);
        }

        private string _error = "";
        public string MensajeError
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        private bool _hayError;
        public bool HayError
        {
            get => _hayError;
            set => SetProperty(ref _hayError, value);
        }

        public ICommand ContinuarCommand { get; }

        private async Task ContinuarAsync()
        {
            HayError = false;
            MensajeError = "";

            var n1 = NombreJugador1?.Trim();
            var n2 = NombreJugador2?.Trim();

            if (string.IsNullOrWhiteSpace(n1) || string.IsNullOrWhiteSpace(n2))
            {
                MostrarError("Ambos nombres son obligatorios.");
                return;
            }
            if (n1.Length < 3 || n2.Length < 3)
            {
                MostrarError("Mínimo 3 caracteres por nombre.");
                return;
            }
            if (string.Equals(n1, n2, StringComparison.OrdinalIgnoreCase))
            {
                MostrarError("Los nombres deben ser distintos.");
                return;
            }

            _servicio.InicializarJugadores(n1, n2);
            await Shell.Current.GoToAsync(nameof(SeleccionRazaPage));

        }

        private void MostrarError(string msg)
        {
            MensajeError = msg;
            HayError = true;
        }
    }
}