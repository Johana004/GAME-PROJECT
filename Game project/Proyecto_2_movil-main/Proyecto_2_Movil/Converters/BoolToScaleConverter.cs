using System.Globalization;

namespace Proyecto_2_Movil.Converters
{
    public class BoolToScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool turno = (bool)value;
            return turno ? 1.15 : 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
