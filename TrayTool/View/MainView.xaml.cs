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

        private readonly string xmlPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TrayTool\\";
        private readonly string xmlName = "TrayTool.xml";


        public MainWindow()
        {
            ViewModel = new MainViewModel
            {
                Items = new ObservableCollection<BaseModel>()
            };
            DataContext = ViewModel;

            if (System.IO.File.Exists(xmlPath + xmlName))
            {
                SerializeWrapper xmlData = FromXML<SerializeWrapper>(xmlPath + xmlName);
                foreach (BaseModel baseModel in xmlData.Elements)
                {
                    FixIncocistentItems(baseModel);
                }
                ViewModel.Items = new ObservableCollection<BaseModel>(xmlData.Elements);
            }

            InitializeComponent();
            Closing += OnWindowClosing;
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (!System.IO.File.Exists(xmlPath + xmlName))
            {
                System.IO.Directory.CreateDirectory(xmlPath);
            }
            string xmlData = ToXML(new SerializeWrapper(new List<BaseModel>(ViewModel.Items)));
            System.IO.File.WriteAllText(xmlPath + xmlName, xmlData);

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

        public T FromXML<T>(string xml)
        {
            using (System.IO.StreamReader stringReader = new System.IO.StreamReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public string ToXML<T>(T obj)
        {
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }

        public XmlAttributeOverrides CreateXmlOverrides()
        {
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();

            overrides.Add(typeof(Item), new XmlAttributes
            {
                XmlType = new XmlTypeAttribute("Item")
            });
            overrides.Add(typeof(Directory), new XmlAttributes
            {
                XmlType = new XmlTypeAttribute("Directory")
            });
            overrides.Add(typeof(Seperator), new XmlAttributes
            {
                XmlType = new XmlTypeAttribute("Seperator")
            });
            return overrides;
        }

        private void FixIncocistentItems(BaseModel baseModel)
        {
            // Fix Parents
            if (baseModel is Directory dir)
            {
                foreach (BaseModel child in dir.Children)
                {
                    Seperator seperator = (Seperator)child;
                    seperator.Parent = dir;
                    FixIncocistentItems(seperator);
                }
            }
        }
    }
}
