using System.Collections.Generic;
using System.Windows.Forms;
using TrayTool.Model;

namespace TrayTool
{
    class MenuGenerator
    {
        public ContextMenuStrip GeneratorMenu(List<BaseModel> models)
        {
            ContextMenuStrip menu = CreateSubMenu(null, models);
            
            return menu;
        }

        private ContextMenuStrip CreateSubMenu(ToolStripMenuItem menu, List<BaseModel> models)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            foreach (BaseModel model in models)
            {
                ToolStripItem menuItem = new ToolStripMenuItem();

                if (model is Directory)
                {
                    menuItem.Tag = model;
                    Directory dir = (Directory) model;
                    menuItem.Text = dir.Name;
                    CreateSubMenu((ToolStripMenuItem)menuItem, new List<BaseModel>(dir.Children));
                }
                else if (model is Seperator)
                {
                    menuItem = new ToolStripSeparator();
                    menuItem.Tag = model;
                }
                else if (model is Item)
                {
                    menuItem.Tag = model;
                    Item item = (Item) model;
                    menuItem.Text = item.Name;

                    
                }

                if (menu != null)
                    menu.DropDownItems.Add(menuItem);
                else
                    contextMenu.Items.Add(menuItem);
            }

            return contextMenu;
        }
    }
}