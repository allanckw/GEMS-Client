using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Gems.UIWPF
{
[ValueConversion( typeof( object ), typeof( int ) )]
	public class NumberToPolarValueConverter : IValueConverter
	{
		public object Convert(
			object value, Type targetType,
			object parameter, CultureInfo culture )
		{
			double number = (double)System.Convert.ChangeType( value, typeof(double) );

			if( number > 300.0 )
				return -1;

			if( number < 300 && number >200 )
				return 0;

			return +1;
		}

		public object ConvertBack(
			object value, Type targetType,
			object parameter, CultureInfo culture )
		{
			throw new NotSupportedException( "ConvertBack not supported" );
		}
	}
}
