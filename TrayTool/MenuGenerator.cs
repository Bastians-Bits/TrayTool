using System.Collections.Generic;
using System.Windows.Forms;
using TrayTool.Model;
using System;
using NLog;

namespace TrayTool
{
    class MenuGenerator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                try
                {
                    Item caller = (Item)((ToolStripMenuItem)sender).Tag;

                    string path = caller.Path;
                    List<Argument> arguments = new List<Argument>(caller.Arguments);

                    string argument = "";
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

                    logger.Debug("Calling {0} with arguments \"{1}\"", path, argument);

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = argument;
                    process.Start();
                } catch(Exception ex)
                {
                    logger.Error(ex, "An error occured while callingthe shortcut");
                }
            }           
        }
    }
}