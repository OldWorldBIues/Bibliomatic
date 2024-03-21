using Bibliomatic_MAUI_App.Models;
using System.Globalization;

namespace Bibliomatic_MAUI_App.Converters
{
    public class CorrectQuestionTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CorrectType correctQuestionType = (CorrectType)value;

            switch (correctQuestionType)
            {
                case CorrectType.Correct:
                    return Colors.Green;

                case CorrectType.PartiallyCorrect:
                    return Colors.Yellow;

                case CorrectType.Incorrect:
                    return Colors.Red;

                default:
                    return Colors.Gray;                    
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
