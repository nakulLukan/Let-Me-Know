using Humanizer;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace LetMeKnow.Converters
{
    public class HumanizeDateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            if (date.Date > DateTime.Now.Date.AddDays(1))
            {
                return date.ToString("dd-MMM-yyyy");
            }
            return date.Date.Humanize(utcDate: false, culture: CultureInfo.InstalledUICulture, dateToCompareAgainst: DateTime.Now.Date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
