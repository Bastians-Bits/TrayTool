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
        public ContextMenuStrip GeneratorMenu(List<BaseModel> models)
        {
            ContextMenuStrip menu = CreateSubMenu(null, models);

            return menu;
        }

        private ContextMenuStrip CreateSubMenu(ToolStripMenuItem menu, List<BaseModel> models)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            foreach (Seperator model in models)
            {
                ToolStripItem menuItem = new ToolStripMenuItem();

                if (model is Directory)
                {
                    Directory dir = (Directory)model;
                    menuItem.Text = dir.Name;
                    CreateSubMenu((ToolStripMenuItem)menuItem, new List<BaseModel>(dir.Children));
                }
                else if (model is Item)
                {
                    Item item = (Item)model;
                    menuItem.Text = item.Name;
                    menuItem.Click += new EventHandler(executeAction);
                }
                else if (model is Seperator) // Seperator has to be asked last, since Directories and Items are Seperators, too
                {
                    menuItem = new ToolStripSeparator();
                }

                menuItem.Tag = model;

                menuItem.Image = model.Image;

                if (menu != null)
                    menu.DropDownItems.Add(menuItem);
                else
                    contextMenu.Items.Add(menuItem);
            }

            return contextMenu;
        }

        private void executeAction(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem && (((ToolStripMenuItem) sender).Tag is Item))
            {
                Item caller = (Item)((ToolStripMenuItem)sender).Tag;

                string path = caller.Path;
                List<Argument> arguments = new List<Argument>(caller.Arguments);

                string argument = "/C";
                argument += " " + path + " ";

                foreach (Argument arg in arguments)
                {
                    argument += arg.Key;
                    if (arg.Concatenator != null && arg.Concatenator.Length > 0)
                        argument += arg.Concatenator;
                    else
                        argument += " ";
                    argument += arg.Value;
                    argument += " ";
                }

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = argument;
                process.StartInfo = startInfo;
                process.Start();
            }           
        }
    }
}