using System;
using System.Globalization;
using System.Windows.Data;

namespace TrayTool
{
    class IndentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int level = (int) value;
            int intend = 20 * level;
            return intend + ", 0, 0, 0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
