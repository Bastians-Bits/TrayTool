using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TrayTool.Converter
{
    /// <summary>
    /// Convert a given image from Bitmap to BitmapImage to use it as WPF component.
    /// </summary>
    class ImageConverter : IValueConverter
    {
    
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] image = (byte[])value;
            return BitmapToImageSource(image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        /// <summary>
        /// Convert the Bitmap-Image to a BitmapImage- Object by reading it into the memory.
        /// </summary>
        /// <param name="bitmap">The Bitmap-Object</param>
        /// <returns>A bitmapImage-Object</returns>
        BitmapImage BitmapToImageSource(byte[] image)
        {
            using (MemoryStream memory = new MemoryStream(image))
            {
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
