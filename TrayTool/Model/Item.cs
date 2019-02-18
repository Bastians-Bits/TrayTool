﻿using System.Collections.ObjectModel;
using System.Drawing;

namespace TrayTool.Model
{
    [System.Serializable()]
    public class Item : AbstractItem
    {
        private string _path;
        private ObservableCollection<Argument> _arguments = new ObservableCollection<Argument>();

        public string Path {
            get => _path;
            set {
                SetProperty(ref _path, value);
                UpdateImage(value);
            }
        }
        public ObservableCollection<Argument> Arguments { get => _arguments; set => SetProperty(ref _arguments, value); }

        public Item()
        {
            Image = CreateImage("/TrayTool;component/Resources/Shortcut.png");
        }

        protected void UpdateImage(string path)
        {
            Icon exeIcon = Icon.ExtractAssociatedIcon(path);
            Image = exeIcon.ToBitmap();
        }
    }
}
