using Proyecto_2_Movil.ViewModels;
namespace Proyecto_2_Movil.Views
{
    public partial class CombatePage : ContentPage
    {
        public CombatePage()
        {
            InitializeComponent();
                BindingContext = new CombateViewModel();
        }
    }
}
