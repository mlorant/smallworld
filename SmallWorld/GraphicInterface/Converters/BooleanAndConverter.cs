using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GraphicInterface
{

    /// <summary>
    /// Returns a boolean by make the logical AND
    /// on two booleans given
    /// </summary>
    class BooleanAndConverter : IMultiValueConverter
    {
        /// <summary>
        /// Conversion of 2 booleans to one
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object value in values)
            {
                if ((value is bool) && (bool)value == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }
}
