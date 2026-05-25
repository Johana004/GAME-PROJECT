using Proyecto_2_Movil.ViewModels;

namespace Proyecto_2_Movil.Views;

public partial class EstadisticasPage : ContentPage
{
    public EstadisticasPage(EstadisticasViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
