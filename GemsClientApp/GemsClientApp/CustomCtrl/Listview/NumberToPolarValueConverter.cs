using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Gems.UIWPF.CustomCtrl
{
    [ValueConversion(typeof(object), typeof(int))]
    public class NumberToPolarValueConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            double number = (double)System.Convert.ChangeType(value, typeof(double));

            if (number == -1)
                return -1;//red

            if (number == 1)
                return +1; //green

            return 0;//normal
        }

        public object ConvertBack(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }

    [ValueConversion(typeof(bool), typeof(int))]
    public class BoolToPolarValueConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            bool isBought = (bool)System.Convert.ChangeType(value, typeof(bool));
            System.Diagnostics.Trace.WriteLine("Debug : is bought = " + isBought + "\n\n");

            if (isBought)
                return 1;//green

            return 0;//normal
        }

        public object ConvertBack(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }
}
