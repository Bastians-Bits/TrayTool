using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using TrayTool.Repository.Model;

namespace TrayTool
{
    public static class SeperatorExtension
    {
        public static Seperator UpdateImage(this Seperator seperator, string path)
        {
            if (path == null || path.Trim().Length == 0) path = RestorePath(seperator);

            seperator.ImagePath = path;

            Bitmap image = CreateImage(path);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                seperator.Image = memoryStream.ToArray();
            }

            return seperator;
        }

        private static Bitmap CreateImage(string uri)
        {
            Bitmap bitmap;

            if (IsResource(ref uri)) {
                Uri bitmapUri = new Uri(uri, UriKind.RelativeOrAbsolute);

                BitmapImage bitmapImage = new BitmapImage(bitmapUri);

                MemoryStream ms = new MemoryStream();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);

                bitmap = new Bitmap(ms);
            } else
            {
                Icon exeIcon = Icon.ExtractAssociatedIcon(uri);
                bitmap = exeIcon.ToBitmap();
            }

            return bitmap;
        }

        private static bool IsResource(ref string source)
        {
            if (source != null && source.StartsWith("/TrayTool;component/"))
            {
                source = "pack://application:,,," + source;
                return true;
            }
            else
            {
                return false;
            }
        }

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

        public static string RestorePath(Seperator seperator)
        {
            if (seperator is Item)
                return "/TrayTool;component/Resources/Shortcut.png";
            if (seperator is Repository.Model.Directory)
                return "/TrayTool;component/Resources/Folder.png";
            else
                return "/TrayTool;component/Resources/Seperator.png";
        }
    }
}
