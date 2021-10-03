using System.Windows;
using System.Collections.ObjectModel;
using TrayTool.Model;
using TrayTool.ViewModel;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Text;
using System.Windows.Controls;
using TrayTool.Repository.Model;
using TrayTool.Repository;

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
            var context = new TrayToolDb();
            ViewModel.Context = context;
            context.Database.EnsureCreated();
            ViewModel.Items = new ObservableCollection<BaseModel>(context.BaseModels);
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
            menuItemExit.Click += new EventHandler(BtnExit_OnClick);
            menu.Items.Add(menuItemExit);

            ToolStripMenuItem menuItemOpenApp = new ToolStripMenuItem("Open App");
            menuItemOpenApp.Click += new EventHandler(BtnOpenApp_OnClick);
            menu.Items.Add(menuItemOpenApp);

            systemTray.ContextMenuStrip = menu;
            systemTray.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.Menu.GetHicon());
            systemTray.Visible = true;

            Hide();

            e.Cancel = true;
        }

        private void BtnExit_OnClick(object sender, EventArgs e)
        {
            systemTray.Visible = false;
            Environment.Exit(0);
        }

        private void BtnOpenApp_OnClick(object sender, EventArgs e)
        {
            Show();
            systemTray.Visible = false;
        }
    }
}
