using Bibliomatic_MAUI_App.Models;
using System.Globalization;

namespace Bibliomatic_MAUI_App.Converters
{
    public class IsCorrectPairsAnswerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var testAnswer = (TestAnswerResponse)value;

            if(testAnswer.Answer == testAnswer.SelectedTestAnswer?.Answer)
            {
                return true;
            }

            return false;            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
