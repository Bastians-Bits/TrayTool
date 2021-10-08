using System.Collections.Generic;
using System.Windows.Forms;
using System;
using NLog;
using TrayTool.Repository.Model;
using System.IO;
using System.Drawing;

namespace TrayTool
{
    /// <summary>
    /// This class creates the context menu for the system tray
    /// </summary>
    class MenuGenerator
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Generate a context menu with a given list of items
        /// </summary>
        /// <param name="models">The list of items</param>
        /// <returns>A context menu</returns>
        public ContextMenuStrip GeneratorMenu(List<BaseModelEntity> models)
        {
            ContextMenuStrip menu = CreateSubMenu(null, models);

            return menu;
        }

        /// <summary>
        /// Create a sub context menu for the items. Will be called recursively to create the folder structure
        /// </summary>
        /// <param name="menu">The current state of the menu. Will be used as reference</param>
        /// <param name="models">The list of models to append</param>
        /// <returns>A sub context for the context menu</returns>
        private ContextMenuStrip CreateSubMenu(ToolStripMenuItem menu, List<BaseModelEntity> models)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            foreach (SeperatorEntity model in models)
            {
                ToolStripItem menuItem = new ToolStripMenuItem();

                if (model is Repository.Model.DirectoryEntity)
                {
                    Repository.Model.DirectoryEntity dir = (Repository.Model.DirectoryEntity)model;
                    menuItem.Text = dir.Name;
                    CreateSubMenu((ToolStripMenuItem)menuItem, new List<BaseModelEntity>(dir.Children));
                }
                else if (model is ItemEntity)
                {
                    ItemEntity item = (ItemEntity)model;
                    menuItem.Text = item.Name;
                    menuItem.Click += new EventHandler(executeAction);
                }
                else if (model is SeperatorEntity) // Seperator has to be asked last, since Directories and Items are Seperators, too
                {
                    menuItem = new ToolStripSeparator();
                }

                menuItem.Tag = model;

                using (var memoreStream = new MemoryStream(model.Image))
                {
                    menuItem.Image = Image.FromStream(memoreStream);
                }

                if (menu != null)
                    menu.DropDownItems.Add(menuItem);
                else
                    contextMenu.Items.Add(menuItem);
            }

            return contextMenu;
        }

        /// <summary>
        /// Handler, if a item in the system tray is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        private void executeAction(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && (item.Tag is ItemEntity caller))
            {
                try
                {
                    string path = caller.Path;
                    List<ArgumentEntity> arguments = new List<ArgumentEntity>(caller.Arguments);

                    string argument = "";
                    foreach (ArgumentEntity arg in arguments)
                    {
                        argument += arg.Key;
                        if (arg.Concatenator != null && arg.Concatenator.Length > 0)
                            argument += arg.Concatenator;
                        else
                            argument += " ";
                        argument += arg.Value;
                        argument += " ";
                    }

                    if (path == null || (path != null && path.Length == 0))
                    {
                        logger.Warn("A called item has no path set. Cancelling the call.");
                        return;
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