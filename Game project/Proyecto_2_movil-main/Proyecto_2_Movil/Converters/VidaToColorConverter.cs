using System.Globalization;

namespace Proyecto_2_Movil.Converters
{
    public class VidaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int vida = (int)value;

            if (vida > 60) return Colors.LimeGreen;
            if (vida > 30) return Colors.Gold;
            return Colors.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
