using System.Globalization;

namespace Proyecto_2_Movil.Converters
{
    public class BoolToOpacityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool turnoInverso = (bool)value;
            return turnoInverso ? 0.6 : 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
