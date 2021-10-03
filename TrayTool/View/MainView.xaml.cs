using System.Windows;
using System.Collections.ObjectModel;
using TrayTool.ViewModel;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using TrayTool.Repository.Model;
using TrayTool.Repository;
using Microsoft.EntityFrameworkCore;

namespace TrayTool.View
{
    /// <summary>
    /// Main class for the view. Also handles the loading and storing of the xml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private readonly NotifyIcon systemTray = new NotifyIcon();
        /// <summary>
        /// The view model behind the view
        /// </summary>
        public MainViewModel ViewModel { get; set; }

        //private readonly string xmlPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TrayTool\\";
        //private readonly string xmlName = "TrayTool.xml";


        public MainWindow()
        {
            ViewModel = new MainViewModel();
            
            DataContext = ViewModel;

            InitializeComponent();
            Closing += OnWindowClosing;
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            List<BaseModel> items = new List<BaseModel>(ViewModel.Items);

            try
            {
               menu = new MenuGenerator().GeneratorMenu(items);
            }
            catch (Exception ex)
            {
                ToolStripMenuItem menuItemException = new ToolStripMenuItem(ex.Message);
                menu.Items.Add(menuItemException);
            }

            ToolStripSeparator seperator = new ToolStripSeparator();
            menu.Items.Add(seperator);

            ToolStripMenuItem menuItemExit = new ToolStripMenuItem("Exit");
            menuItemExit.Click += new EventHandler((sender, e) => 
            {
                systemTray.Visible = false;
                Environment.Exit(0);
            });
            menu.Items.Add(menuItemExit);

            ToolStripMenuItem menuItemOpenApp = new ToolStripMenuItem("Open App");
            menuItemOpenApp.Click += new EventHandler((sender, e) => 
            {
                Show();
                systemTray.Visible = false;
            });
            menu.Items.Add(menuItemOpenApp);

            systemTray.ContextMenuStrip = menu;
            systemTray.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.Menu.GetHicon());
            systemTray.Visible = true;

            Hide();

            e.Cancel = true;
        }
    }
}
