using Proyecto_2_Movil.Views;

namespace Proyecto_2_Movil
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegistroJugadoresPage), typeof(RegistroJugadoresPage));
            Routing.RegisterRoute(nameof(SeleccionRazaPage), typeof(SeleccionRazaPage));
            Routing.RegisterRoute(nameof(SeleccionArmaPage), typeof(SeleccionArmaPage));
            Routing.RegisterRoute(nameof(CombatePage), typeof(CombatePage));
            Routing.RegisterRoute(nameof(EstadisticasPage), typeof(EstadisticasPage));
            Routing.RegisterRoute(nameof(InicioPage), typeof(InicioPage));

        }
    }
}
