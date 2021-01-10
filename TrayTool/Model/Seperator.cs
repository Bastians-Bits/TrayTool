using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Xml.Serialization;

namespace TrayTool.Model
{
    /// <summary>
    /// The class provides all function to display an item properly
    /// </summary>
    [Serializable]
    public class Seperator : BaseModel
    {
        [XmlIgnore]
        private Bitmap _image;
        [XmlIgnore]
        private Directory _parent;
        [XmlIgnore]
        private bool _isSelected = false;
        [XmlIgnore]
        private bool _isExpanded = false;

        /// <summary>
        /// The image of the item
        /// </summary>
        [XmlIgnore]
        public Bitmap Image { get => _image; set => SetProperty(ref _image, value); }
        /// <summary>
        /// The parent of the item. If it is a root item, the parent is set to null
        /// </summary>
        [XmlIgnore]
        public Directory Parent { get => _parent; set => SetProperty(ref _parent, value); }
        /// <summary>
        /// Whether or not the item is selected
        /// </summary>
        [XmlIgnore]
        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }
        /// <summary>
        /// Whether or not the item is expanded
        /// </summary>
        [XmlIgnore]
        public bool IsExpanded { get => _isExpanded; set => SetProperty(ref _isExpanded, value); }

        public Seperator() 
        {
            Image = CreateImage("/TrayTool;component/Resources/Seperator.png");
        }

        /// <summary>
        /// Create a bitmap from the given uri
        /// </summary>
        /// <param name="uri">The uri to the image</param>
        /// <returns>A bitmap-image</returns>
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

        /// <summary>
        /// Whether the image locates in the applications resources or the filesystem
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks whether a given item is an ancestor of the calling item is.
        /// </summary>
        /// <param name="item">The presumably ancestor</param>
        /// <returns>True, of is an ancestor, otherwise false</returns>
        public bool IsAncestor(Seperator item)
        {
            Seperator parent = Parent;

            while (parent != null )
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
