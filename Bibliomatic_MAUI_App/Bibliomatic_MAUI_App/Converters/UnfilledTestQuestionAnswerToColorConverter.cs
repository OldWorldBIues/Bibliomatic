using System.Globalization;

namespace Bibliomatic_MAUI_App.Converters
{
    public class UnfilledTestQuestionAnswerToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool testQuestionAnswerUnfilled = (bool)value;

            if(testQuestionAnswerUnfilled)
            {
                return Colors.Orange;
            }

            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
