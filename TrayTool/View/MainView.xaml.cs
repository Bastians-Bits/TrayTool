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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private NotifyIcon systemTray = new NotifyIcon();

        MainViewModel viewModel;

        private string xmlPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TrayTool.xml";


        public MainWindow()
        {
            viewModel = new MainViewModel();
            viewModel.Items = new ObservableCollection<BaseModel>();
            DataContext = viewModel;

            if (System.IO.File.Exists(xmlPath))
            {
                SerializeWrapper xmlData = FromXML<SerializeWrapper>(xmlPath);
                foreach (BaseModel baseModel in xmlData.elements)
                {
                    FixIncocistentItems(baseModel);
                }
                viewModel.Items = new ObservableCollection<BaseModel>(xmlData.elements);
            }

            InitializeComponent();
            Closing += OnWindowClosing;
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            string xmlData = ToXML(new SerializeWrapper(new List<BaseModel>(viewModel.Items)));
            System.IO.File.WriteAllText(xmlPath, xmlData);

            ContextMenuStrip menu = new ContextMenuStrip();

            List<BaseModel> items = new List<BaseModel>(viewModel.Items);

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

        private TreeViewItem GetContainerFromStuff(Seperator stuff)
        {
            Stack<Seperator> _stack = new Stack<Seperator>();
            _stack.Push(stuff);
            Seperator parent = stuff.Parent;

            while (parent != null)
            {
                _stack.Push(parent);
                parent = parent.Parent;
            }

            System.Windows.Controls.ItemsControl container = treeView;
            while ((_stack.Count > 0) && (container != null))
            {
                BaseModel top = _stack.Pop();
                container = (System.Windows.Controls.ItemsControl)container.ItemContainerGenerator.ContainerFromItem(top);
            }

            return container as System.Windows.Controls.TreeViewItem;
        }

        private System.Windows.Controls.TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item. 
            System.Windows.Controls.TreeViewItem container = element as System.Windows.Controls.TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = System.Windows.Media.VisualTreeHelper.GetParent(element) as UIElement;
                container = element as System.Windows.Controls.TreeViewItem;
            }

            return container;
        }

        
        private Point _lastMouseDown;


        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(treeView);
            }
        }

        
        private void TreeView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(treeView);

                // Note: This should be based on some accessibility number and not just 2 pixels 
                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 2.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 2.0))
                {
                    Seperator selectedItem = (Seperator)treeView.SelectedItem;
                    if (selectedItem != null)
                    {
                        System.Windows.Controls.TreeViewItem container = GetContainerFromStuff(selectedItem);
                        if (container != null)
                        {
                            System.Windows.DragDropEffects finalDropEffect = DragDrop.DoDragDrop(container, selectedItem, System.Windows.DragDropEffects.Move);
                        }
                    }
                }
            }
        }
        
        private void TreeView_Drop(object sender, System.Windows.DragEventArgs e)
        {
            e.Effects = System.Windows.DragDropEffects.None;
            e.Handled = true;

            // Verify that this is a valid drop and then store the drop target 
            System.Windows.Controls.TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            if (container != null)
            {
                Seperator sourceStuff = (Seperator)e.Data.GetData(e.Data.GetFormats()[0]);
                Seperator targetStuff = (Seperator)container.Header;
                if ((sourceStuff != null) && (targetStuff != null))
                {
                    if (sourceStuff != targetStuff)
                    {
                        // Remove from the old 
                        if (sourceStuff.Parent != null)
                        {
                            sourceStuff.Parent.Children.Remove(sourceStuff);
                        }
                        else
                        {
                            viewModel.Items.Remove(sourceStuff);
                        }
                        // Add it to the new one
                        if (targetStuff is Directory)
                        {
                            // It is dragged to a dir, add it to the dir
                            ((Directory)targetStuff).Children.Add(sourceStuff);
                            sourceStuff.Parent = (Directory)targetStuff;
                        }
                        else
                        {
                            // It is not dragged to a dir, add it to the parent dir or create a new root element
                            if (targetStuff.Parent != null)
                            {
                                int index = targetStuff.Parent.Children.IndexOf(targetStuff) + 1;

                                targetStuff.Parent.Children.Insert(index, sourceStuff);
                                sourceStuff.Parent = targetStuff.Parent;
                            }
                            else
                            {
                                viewModel.Items.Add(sourceStuff);
                                sourceStuff.Parent = null;
                            }
                        }
                    }
                }
            }
            else
            {
                Seperator sourceStuff = (Seperator)e.Data.GetData(e.Data.GetFormats()[0]);

                if (sourceStuff.Parent != null)
                {
                    sourceStuff.Parent.Children.Remove(sourceStuff);
                    sourceStuff.Parent = null;
                    viewModel.Items.Add(sourceStuff);
                } 
                else
                {
                    viewModel.Items.Move(viewModel.Items.IndexOf(sourceStuff), viewModel.Items.Count - 1);
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewModel.TreeView_Selected = (Seperator) treeView.SelectedItem;
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

        private void FixIncocistentItems(BaseModel baseModel)
        {
            // Fix Parents
            if (baseModel is Directory)
            {
                Directory dir = (Directory) baseModel;
                foreach(BaseModel child in dir.Children)
                {
                    Seperator seperator = (Seperator)child;
                    seperator.Parent = dir;
                    FixIncocistentItems(seperator);
                }
            }
        }

        private void TreeView_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Movement move = new Movement()
            {
                Item = (BaseModel)treeView.SelectedItem
            };
            if (e.Key == Key.A)
            {
                // Move Left
                move.Moevement = Movement.Direction.LEFT;
            } else if (e.Key == Key.D)
            {
                // Move Right
                move.Moevement = Movement.Direction.RIGHT;
            } else if (e.Key == Key.W)
            {
                // Move Up
                move.Moevement = Movement.Direction.UP;
            } else if (e.Key == Key.S)
            {
                // Move Down
                move.Moevement = Movement.Direction.DOWN;
            }
            viewModel.Move(move);

            SetSelectItem(move.Item);
        }

        private bool SetSelectItem(BaseModel item)
        {
            // gets all nodes from the TreeView
            // Not the best way, but there won't be at any point more than 30 nodes
            Collection<TreeViewItem> allTreeContainers = GetAllItemContainers(treeView);
            foreach (TreeViewItem treeViewItem in allTreeContainers)
            {
                if (treeViewItem.DataContext == item)
                {
                    // Make sure the new parent is expanded
                    if (treeViewItem.DataContext is Seperator)
                    {
                        Seperator parentItem = (treeViewItem.DataContext as Seperator).Parent;
                        foreach (TreeViewItem parentTreeViewItem in allTreeContainers)
                        {
                            if (parentTreeViewItem.DataContext == parentItem)
                            {
                                parentTreeViewItem.IsExpanded = true;
                            }
                        }
                    }

                    // Set the new moved item to selected
                    treeViewItem.IsSelected = true;
                    return true;
                }
            }
            return false;
        }

        private Collection<TreeViewItem> GetAllItemContainers(ItemsControl itemsControl)
        {
            Collection<TreeViewItem> allItems = new Collection<TreeViewItem>();
            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                // try to get the item Container   
                TreeViewItem childItemContainer = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                // the item container maybe null if it is still not generated from the runtime   
                if (childItemContainer != null)
                {
                    allItems.Add(childItemContainer);
                    Collection<TreeViewItem> childItems = GetAllItemContainers(childItemContainer);
                    foreach (TreeViewItem childItem in childItems)
                    {
                        allItems.Add(childItem);
                    }
                }
            }
            return allItems;
        }
    }
}
