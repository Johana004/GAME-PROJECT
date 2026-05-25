using Microsoft.Extensions.Logging;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.ViewModels;
using Proyecto_2_Movil.Views;

namespace Proyecto_2_Movil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ✅ Registrar servicios y VMs
            builder.Services.AddSingleton<PersistenciaService>();
            builder.Services.AddTransient<SeleccionRazaViewModel>();
            builder.Services.AddTransient<EstadisticasPage>();
            builder.Services.AddTransient<EstadisticasViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}