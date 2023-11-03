using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GFMS.Services
{
    public class NullableIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int?)
            {

                var intValue = (int?)value;
                return intValue.HasValue ? intValue.Value.ToString() : string.Empty;
            }
            return string.Empty;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var stringValue = (string)value;
                int? myValue = null;
                if (int.TryParse(stringValue, out int parsedInt))
                {
                    myValue = parsedInt;
                }
                return myValue != null ? myValue : null;
            }
            return null;
        }
    }
}
