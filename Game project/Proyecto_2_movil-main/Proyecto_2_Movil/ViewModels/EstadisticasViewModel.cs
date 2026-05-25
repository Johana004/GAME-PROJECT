using System.Collections.ObjectModel;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;
using System.Linq;

namespace Proyecto_2_Movil.ViewModels
{
    public class EstadisticasViewModel : BaseViewModel
    {
        private readonly PersistenciaService _persistencia;

        public ObservableCollection<EstadisticaJugador> Jugadores { get; } = new();

        public EstadisticasViewModel(PersistenciaService persistencia)
        {
            _persistencia = persistencia;
            CargarEstadisticas();
        }

        private void CargarEstadisticas()
        {
            Jugadores.Clear();

            var lista = _persistencia.ObtenerEstadisticasJugadores();

            foreach (var item in lista)
                Jugadores.Add(item);
        }
    }
}
