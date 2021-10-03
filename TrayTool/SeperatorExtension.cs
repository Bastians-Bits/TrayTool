using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using TrayTool.Repository.Model;

namespace TrayTool
{
    public static class SeperatorExtension
    {
        public static Seperator UpdateImage(this Seperator seperator, string path)
        {
            seperator.ImagePath = path;

            Image image = CreateImage(path);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                seperator.Image = memoryStream.ToArray();
            }

            return seperator;
        }

        private static Image CreateImage(string uri)
        {
            Uri bitmapUri = new Uri(IsResource(uri), UriKind.RelativeOrAbsolute);

            BitmapImage bitmapImage = new BitmapImage(bitmapUri);

            MemoryStream ms = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(ms);

            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        /// <summary>
        /// Whether the image locates in the applications resources or the filesystem
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string IsResource(string source)
        {
            if (source != null && source.StartsWith("/TrayTool;component/"))
            {
                return "pack://application:,,," + source;
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Checks whether a given item is an ancestor of the calling item is.
        /// </summary>
        /// <param name="item">The presumably ancestor</param>
        /// <returns>True, of is an ancestor, otherwise false</returns>
        public static bool IsAncestor(this Seperator seperator, Seperator item)
        {
            Seperator parent = seperator.Parent;

            while (parent != null)
            {
                if (parent == item)
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }
    }
}
