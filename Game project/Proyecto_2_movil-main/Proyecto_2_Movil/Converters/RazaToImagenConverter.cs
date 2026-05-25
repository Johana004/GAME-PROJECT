using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Proyecto_2_Movil.Converters
{
    public class RazaToImagenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string raza = value?.ToString() ?? "";

            return raza switch
            {
                "Humano" => "humano.jpeg",
                "Elfo" => "elfo.jpeg",
                "Elfo (Agua)" => "elfo_agua.jpeg",
                "Orco" => "orco.jpeg",
                "Bestia" => "bestia.jpeg",
                _ => "personaje_default.jpeg"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
