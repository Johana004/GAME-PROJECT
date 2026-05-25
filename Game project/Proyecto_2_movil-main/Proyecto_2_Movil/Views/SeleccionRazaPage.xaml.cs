// Views/SeleccionRazaPage.xaml.cs
using Microsoft.Maui.Controls;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.ViewModels;

namespace Proyecto_2_Movil.Views;

public partial class SeleccionRazaPage : ContentPage
{
    public SeleccionRazaPage(PersistenciaService servicio)
    {
        InitializeComponent();
        BindingContext = new SeleccionRazaViewModel(servicio);
    }

    public SeleccionRazaPage() : this(
        App.Current.Handler?.MauiContext?.Services?.GetRequiredService<PersistenciaService>()
        ?? new PersistenciaService())
    { }
}