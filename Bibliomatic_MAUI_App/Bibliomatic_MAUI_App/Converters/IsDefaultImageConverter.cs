using System.Globalization;

namespace Bibliomatic_MAUI_App.Converters
{
    public class IsDefaultImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = (string)value;
            return string.IsNullOrEmpty(source) || source == "attach_element.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
