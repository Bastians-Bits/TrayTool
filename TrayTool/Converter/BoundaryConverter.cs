using System;
using System.Globalization;
using System.Windows.Data;

namespace TrayTool
{
    /// <summary>
    /// This class adds a given padding to the object by substring the parameter from the width.
    /// </summary>
    public class BoundaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double) value;
            double padding = double.Parse((string) parameter);
            width -= padding;
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
