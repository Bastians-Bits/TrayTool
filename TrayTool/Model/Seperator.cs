using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Xml.Serialization;

namespace TrayTool.Model
{
    [Serializable]
    public class Seperator : BaseModel
    {
        [XmlIgnore]
        private Bitmap _image;
        [XmlIgnore]
        private Directory _parent;
        [XmlIgnore]
        public Bitmap Image { get => _image; set => SetProperty(ref _image, value); }
        [XmlIgnore]
        public Directory Parent { get => _parent; set => SetProperty(ref _parent, value); }
        [XmlIgnore]
        public bool IsExpanded { get; set; } = false;
        [XmlIgnore]
        public bool IsSelected { get; set; } = false;

        public Seperator()
        {
            Image = CreateImage("/TrayTool;component/Resources/Seperator.png");
        }

        protected Bitmap CreateImage(string uri)
        {
            Uri bitmapUri = new Uri(IsResource(uri), UriKind.RelativeOrAbsolute);

            BitmapImage bitmapImage = new BitmapImage(bitmapUri);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(ms);

            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        private string IsResource(string source)
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
    }
}
