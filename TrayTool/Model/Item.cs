using System.Collections.ObjectModel;
using System.Drawing;

namespace TrayTool.Model
{
    /// <summary>
    /// A full-fledged item. Is contains all information to invoke a program with cli arguments
    /// </summary>
    [System.Serializable()]
    public class Item : AbstractItem
    {
        private string _path;
        private ObservableCollection<Argument> _arguments = new ObservableCollection<Argument>();

        /// <summary>
        /// The path to the executable
        /// </summary>
        public string Path {
            get => _path;
            set {
                SetProperty(ref _path, value);
                UpdateImage(value);
            }
        }

        /// <summary>
        /// A list of cli arguments
        /// </summary>
        public ObservableCollection<Argument> Arguments { get => _arguments; set => SetProperty(ref _arguments, value); }

        public Item()
        {
            Image = CreateImage("/TrayTool;component/Resources/Shortcut.png");
        }

        /// <summary>
        /// Extract the image from the executable and set is as new item image
        /// </summary>
        /// <param name="path">The path to the executable</param>
        protected void UpdateImage(string path)
        {
            Icon exeIcon = Icon.ExtractAssociatedIcon(path);
            Image = exeIcon.ToBitmap();
        }
    }
}
