using System;
using System.Globalization;
using Xamarin.Forms;

namespace LetMeKnow.Converters
{
    public class VaccineSlotTextValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = parameter.ToString();
            if (type != "X")
            {
                int nos = (int)value;
                if (nos > 0)
                {
                    return string.Concat(nos, " slots");
                }

                return string.Empty;
            }
            else
            {
                int nos = (int)value;
                if (nos > 0)
                {
                    return string.Empty;
                }

                return "X";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
