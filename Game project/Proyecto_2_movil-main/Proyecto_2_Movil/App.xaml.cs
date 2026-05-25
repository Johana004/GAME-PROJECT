using Proyecto_2_Movil.Services;

namespace Proyecto_2_Movil;

public partial class App : Application
{
    private readonly PersistenciaService _persistencia;

    // 👇 Inyección de dependencias: MAUI crea App y nos pasa PersistenciaService
    public App(PersistenciaService persistencia)
    {
        InitializeComponent();

        _persistencia = persistencia;

        // Cargar JSON al inicio (no importa esperar aquí)
        _ = _persistencia.InicializarAsync();

        MainPage = new AppShell();
    }
}
