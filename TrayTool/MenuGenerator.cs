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
                if (model is Directory)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Tag = model;
                    Directory dir = (Directory) model;
                    menuItem.Text = dir.Name;
                    CreateSubMenu(menuItem, new List<BaseModel>(dir.Children));

                    if (menu != null)
                        menu.DropDownItems.Add(menuItem);
                    else
                        contextMenu.Items.Add(menuItem);
                }
                else if (model is Seperator)
                {
                    ToolStripSeparator menuItem = new ToolStripSeparator();
                    menuItem.Tag = model;
                    if (menu != null)
                        menu.DropDownItems.Add(menuItem);
                    else
                        contextMenu.Items.Add(menuItem);
                }
                else if (model is Item)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Tag = model;
                    Item item = (Item) model;
                    menuItem.Text = item.Name;

                    if (menu != null)
                        menu.DropDownItems.Add(menuItem);
                    else
                        contextMenu.Items.Add(menuItem);
                }
            }

            return contextMenu;
        }
    }
}