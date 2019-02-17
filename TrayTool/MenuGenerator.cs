using System.Collections.Generic;
using System.Windows.Forms;
using TrayTool.Model;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace TrayTool
{
    class MenuGenerator
    {
        public ContextMenuStrip GeneratorMenu(List<Seperator> models)
        {
            ContextMenuStrip menu = CreateSubMenu(null, models);

            return menu;
        }

        private ContextMenuStrip CreateSubMenu(ToolStripMenuItem menu, List<Seperator> models)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            foreach (Seperator model in models)
            {
                ToolStripItem menuItem = new ToolStripMenuItem();

                if (model is Directory)
                {
                    Directory dir = (Directory)model;
                    menuItem.Text = dir.Name;
                    CreateSubMenu((ToolStripMenuItem)menuItem, new List<Seperator>(dir.Children));
                }
                else if (model is Item)
                {
                    Item item = (Item)model;
                    menuItem.Text = item.Name;
                }
                else if (model is Seperator) // Seperator has to be asked last, since Directories and Items are Seperators, too
                {
                    menuItem = new ToolStripSeparator();
                }

                menuItem.Tag = model;

                menuItem.Image = CreateImage(model.Image);

                if (menu != null)
                    menu.DropDownItems.Add(menuItem);
                else
                    contextMenu.Items.Add(menuItem);
            }

            return contextMenu;
        }

        private Bitmap CreateImage(string uri)
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