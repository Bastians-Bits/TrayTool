using System;
using System.Globalization;
using System.Windows.Data;

namespace TrayTool
{
    public class BoundaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double) value;
            double padding = double.Parse((string) parameter);
            width = width - padding;
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
