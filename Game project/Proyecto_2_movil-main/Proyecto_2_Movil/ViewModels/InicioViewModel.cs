using System.Windows.Input;
using Proyecto_2_Movil.Views;

namespace Proyecto_2_Movil.ViewModels
{
    public partial class InicioViewModel : BaseViewModel
    {
        public ICommand IniciarCommand { get; }
        public ICommand EstadisticasCommand { get; }

        public InicioViewModel()
        {
            IniciarCommand = new Command(IrARegistro);
            EstadisticasCommand = new Command(IrAEstadisticas);
        }

        private async void IrARegistro()
        {
            await Shell.Current.GoToAsync(nameof(RegistroJugadoresPage));
        }

        private async void IrAEstadisticas()
        {
            await Shell.Current.GoToAsync(nameof(EstadisticasPage));
        }
    }
}
