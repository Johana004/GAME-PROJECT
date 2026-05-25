// Views/RegistroJugadoresPage.xaml.cs
using Microsoft.Maui.Controls;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.ViewModels;

namespace Proyecto_2_Movil.Views;

public partial class RegistroJugadoresPage : ContentPage
{
    public RegistroJugadoresPage(PersistenciaService persistenciaService)
    {
        InitializeComponent();
        BindingContext = new RegistroJugadoresViewModel(persistenciaService);
    }

    public RegistroJugadoresPage() : this(App.Current.Handler.MauiContext.Services.GetRequiredService<PersistenciaService>())
    {
    }
}