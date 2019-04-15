using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TrayTool.Model;
using TrayTool.View;
using TrayTool.ViewModel;

namespace TrayTool.UIControlls
{
    /// <summary>
    /// Interaktionslogik für ExtendetTreeView.xaml
    /// </summary>
    public partial class ExtendetTreeView : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ExtendetTreeView()
        {
            InitializeComponent();
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

            ItemsControl container = treeView;
            while ((_stack.Count > 0) && (container != null))
            {
                BaseModel top = _stack.Pop();
                container = (ItemsControl)container.ItemContainerGenerator.ContainerFromItem(top);
            }

            return container as TreeViewItem;
        }

        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item. 
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
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


        private void TreeView_MouseMove(object sender, MouseEventArgs e)
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
                        TreeViewItem container = GetContainerFromStuff(selectedItem);
                        if (container != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(container, selectedItem, DragDropEffects.Move);
                        }
                    }
                }
            }
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            // Verify that this is a valid drop and then store the drop target 
            TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            if (container != null)
            {
                Seperator sourceStuff = (Seperator)e.Data.GetData(e.Data.GetFormats()[0]);
                Seperator targetStuff = (Seperator)container.Header;
                if ((sourceStuff != null) && (targetStuff != null))
                {
                    // source can't be target and source can't be an ancestor of target
                    if (sourceStuff != targetStuff && !targetStuff.IsAncestor(sourceStuff))
                    {
                        // Remove from the old 
                        if (sourceStuff.Parent != null)
                        {
                            sourceStuff.Parent.Children.Remove(sourceStuff);
                        }
                        else
                        {
                            ((MainViewModel)DataContext).Items.Remove(sourceStuff);
                        }
                        // Add it to the new one
                        if (targetStuff is Directory)
                        {
                            // It is dragged to a dir, add it to the dir
                            ((Directory)targetStuff).Children.Add(sourceStuff);
                            sourceStuff.Parent = (Directory)targetStuff;
                            ((Directory)targetStuff).IsExpanded = true;
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
                                ((MainViewModel)DataContext).Items.Add(sourceStuff);
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
                    ((MainViewModel)DataContext).Items.Add(sourceStuff);
                }
                else
                {
                    ((MainViewModel)DataContext).Items.Move(
                        ((MainViewModel)DataContext).Items.IndexOf(sourceStuff),
                        ((MainViewModel)DataContext).Items.Count - 1);
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ((MainViewModel)DataContext).TreeView_Selected = (Seperator)treeView.SelectedItem;
        }

        private void TreeView_KeyUp(object sender, KeyEventArgs e)
        {
            Movement move = new Movement()
            {
                Item = ((MainViewModel)DataContext).TreeView_Selected
            };

            switch (e.Key)
            {
                case Key.A:
                    move.Moevement = Movement.Direction.LEFT;
                    break;
                case Key.D:
                    move.Moevement = Movement.Direction.RIGHT;
                    break;
                case Key.W:
                    move.Moevement = Movement.Direction.UP;
                    break;
                case Key.S:
                    move.Moevement = Movement.Direction.DOWN;
                    break;
                default:
                    move.Moevement = Movement.Direction.NONE;
                    break;
            }
            ((MainViewModel)DataContext).Move(move);
        }
    }
}
