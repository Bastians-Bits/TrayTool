using System.Windows;
using System.Collections.ObjectModel;
using TrayTool.Model;
using TrayTool.ViewModel;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TrayTool.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private NotifyIcon systemTray = new NotifyIcon();

        MainViewModel viewModel;


        public MainWindow()
        {
            viewModel = new MainViewModel();

            Directory dir1 = new Directory() { Name = "Dir 1" };
            Item item1 = new Item()
            {
                Name = "Item 1",
                Parent = dir1,
                Arguments = new ObservableCollection<Argument>()
                {
                    new Argument() { Key = "Test", Value = "Test"  }
                }
            };
            dir1.Children.Add(item1);
            Item item2 = new Item() { Name = "Item 2", Parent = dir1 };
            dir1.Children.Add(item2);

            Seperator seperator = new Seperator() { Parent = dir1 };
            dir1.Children.Add(seperator);

            Directory dir2 = new Directory() { Name = "Dir 2", Parent = dir1 };
            dir1.Children.Add(dir2);
            Item item3 = new Item() { Name = "Item 3", Parent = dir2 };
            dir2.Children.Add(item3);
            Item item4 = new Item() { Name = "Item 4", Parent = dir2 };
            dir2.Children.Add(item4);


            Directory dir3 = new Directory() { Name ="Dir 3" };
            Item item5 = new Item() { Name = "Item 5", Parent = dir3 };
            dir3.Children.Add(item5);
            Item item6 = new Item() { Name = "Item 6", Parent = dir3 };
            dir3.Children.Add(item6);



            viewModel.Items = new ObservableCollection<Seperator>();
            viewModel.Items.Add(dir1);
            viewModel.Items.Add(dir3);

            DataContext = viewModel;

            InitializeComponent();
            Closing += OnWindowClosing;
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            List<Seperator> items = new List<Seperator>(viewModel.Items);

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

        private System.Windows.Controls.TreeViewItem GetContainerFromStuff(Seperator stuff)
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
    }
}
