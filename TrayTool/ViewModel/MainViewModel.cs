using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using TrayTool.Model;

namespace TrayTool.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BaseModel> _items;

        public ICommand ButtonAdd { get; private set; }
        public ICommand ButtonRemove { get; private set; }
        public ICommand ButtonBrowserPath { get; private set; }
        public ICommand TreeView { get; private set; }
        public ICommand CbChooser { get; private set; }

        public ObservableCollection<BaseModel> Items { get => _items; set => SetProperty(ref _items, value); }
        public ObservableCollection<ArguementTemplate> ArguementTemplates { get; set; }

        public Seperator TreeView_Selected { get; set; }
        public int CbAddChooser_Selected { get; set; }

        public MainViewModel()
        {
            ButtonAdd = new DelegateCommand(ButtonAddClick);
            ButtonRemove = new DelegateCommand(ButtonRemoveClick);
            ButtonBrowserPath = new DelegateCommand(ButtonBrowserPathClick);
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

        private void ButtonAddClick(object commandParamter)
        {
            logger.Trace("ButtonAddClick called");
            if (TreeView_Selected != null)
            {
                if (TreeView_Selected is Directory)
                {
                    ((Directory)TreeView_Selected).Children.Add(CreateNewInstance(CbAddChooser_Selected, (Directory)TreeView_Selected));
                    logger.Debug("Added instance {instance} to the directory {directory}", CbAddChooser_Selected, ((Directory)TreeView_Selected).Name);
                }
                else
                {
                    // Search for Parent dir and add it there
                    if (TreeView_Selected.Parent != null)
                    {
                        int index = TreeView_Selected.Parent.Children.IndexOf(TreeView_Selected) + 1;

                        // Add it to the parent                   
                        TreeView_Selected.Parent.Children.Insert(index, CreateNewInstance(CbAddChooser_Selected, TreeView_Selected.Parent));
                        logger.Debug("Added instance {instance} to the directory {directory}", CbAddChooser_Selected, ((Directory)TreeView_Selected).Parent.Name);
                    }
                    else
                    {
                        int index = Items.IndexOf(TreeView_Selected);

                        // No parent, the currently selected is a root element
                        Items.Insert(index + 1, CreateNewInstance(CbAddChooser_Selected, null));
                        logger.Debug("Added instance {instance} to the directory {directory}", CbAddChooser_Selected, "ROOT");
                    }
                }
            }
            else
            {
                // No parent dir, we have to create a new root element
                Items.Add(CreateNewInstance(CbAddChooser_Selected, null));
                logger.Debug("Added instance {instance} to the directory {directory}", CbAddChooser_Selected, "ROOT");
            }
            logger.Trace("ButtonAddClick left");
        }

        private void ButtonRemoveClick(object commandParamter)
        {
            logger.Trace("ButtonRemoveClick called");
            if (TreeView_Selected != null)
            {
                //TODO Check for children and warn
                if (TreeView_Selected.Parent != null)
                {
                    logger.Debug("Delete item {0}", TreeView_Selected);
                    TreeView_Selected.Parent.Children.Remove(TreeView_Selected);
                }
                else
                {
                    // We have no parent, it has to be a root element
                    for (int i = 0; i < Items.Count; i++)
                    {
                        // We won't use CompareTo, since the values are not relevant (duplicates are allowes), but the instance won't lie
                        if (Items[i] == TreeView_Selected)
                        {
                            logger.Debug("Delete item {0}", Items[i].ToString());
                            Items.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            logger.Trace("ButtonRemoveClick left");
        }

        public Seperator CreateNewInstance(int target, Directory parent)
        {
            switch (target)
            {
                case 0:
                    return new Item()
                    {
                        Name = "New Item",
                        Parent = parent
                    };
                case 1:
                    return new Directory()
                    {
                        Name = "New Directory",
                        Parent = parent
                    };
                case 2:
                    return new Seperator()
                    {
                        Parent = parent
                    };
            }
            return null;
        }

        private void ButtonBrowserPathClick(object commandParamter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Item item = (Item)TreeView_Selected;
                item.Path = openFileDialog.FileName;
            }
        }

        public void Move(Movement movement)
        {
            switch (movement.Moevement)
            {
                case Movement.Direction.UP:
                    MoveUp(movement.Item);
                    break;
                case Movement.Direction.DOWN:
                    MoveDown(movement.Item);
                    break;
                case Movement.Direction.LEFT:
                    MoveLeft(movement.Item);
                    break;
                case Movement.Direction.RIGHT:
                    MoveRight(movement.Item, movement.Index);
                    break;
            }
        }

        private void MoveUp(BaseModel item)
        {
            if (IsRoot(item))
            {
                int index = Items.IndexOf(item) - 1;
                if (index >= 0)
                {
                    Items.Move(Items.IndexOf(item), Items.IndexOf(item) - 1);
                }
            }
            else
            {
                if (item is Seperator)
                {
                    ObservableCollection<BaseModel> siblings = ((Seperator)item).Parent.Children;
                    int index = siblings.IndexOf(item) - 1;
                    if (index >= 0)
                    {
                        siblings.Move(siblings.IndexOf(item), siblings.IndexOf(item) - 1);
                    }
                }
            }
        }

        private void MoveDown(BaseModel item)
        {

            if (IsRoot(item))
            {
                int index = Items.IndexOf(item) + 1;
                if (index <= Items.Count - 1)
                {
                    Items.Move(Items.IndexOf(item), Items.IndexOf(item) + 1);
                }
            }
            else
            {
                if (item is Seperator)
                {
                    ObservableCollection<BaseModel> siblings = ((Seperator)item).Parent.Children;
                    int index = siblings.IndexOf(item) + 1;
                    if (index <= siblings.Count - 1)
                    {
                        siblings.Move(siblings.IndexOf(item), siblings.IndexOf(item) + 1);
                    }
                }
            }
        }

        private void MoveLeft(BaseModel item)
        {
            if (!IsRoot(item))
            {
                if (item is Seperator)
                {
                    Directory oldParent = ((Seperator)item).Parent;
                    Directory newParent = oldParent.Parent;

                    // Remove old assignment
                    oldParent.Children.Remove(item);
                    ((Seperator)item).Parent = null;

                    if (newParent != null)
                    {
                        // Create new Assignment
                        newParent.Children.Insert(newParent.Children.IndexOf(oldParent), item);
                        ((Seperator)item).Parent = newParent;
                    }
                    else
                    {
                        // Means here, we have a new root node
                        Items.Insert(Items.IndexOf(oldParent), item);
                    }
                }
            }
        }

        private void MoveRight(BaseModel item, int index = 0)
        {
            Directory newParent = NearestDirectory(item);
            
            if (newParent != null)
            {
                newParent.IsExpanded = true;
                if (item is Seperator)
                {
                    if (IsRoot(item))
                    {
                        Items.Remove(item);
                    }
                    else
                    {
                        ((Seperator)item).Parent.Children.Remove(item);
                    }
                    ((Seperator)item).Parent = newParent;
                }
                newParent.Children.Insert(0, item);
            }
        }

        public bool IsRoot(BaseModel item)
        {
            if (item is Seperator)
            {
                if (((Seperator)item).Parent == null)
                {
                    return true;
                }
            }
            return false;
        }

        public Directory NearestDirectory(BaseModel item)
        {
            int myIndex;
            if (IsRoot(item))
            {
                // Get the next directory from the list
                myIndex = Items.IndexOf(item);
                if (item is Directory) myIndex++; 
                for (; myIndex < Items.Count; myIndex++)
                {
                    if (Items[myIndex] is Directory)
                    {
                        return (Directory)Items[myIndex];
                    }
                }
            }
            else
            {
                // Get the next directory from the siblings
                myIndex = ((Seperator)item).Parent.Children.IndexOf(item);
                if (item is Directory) myIndex++;
                for (; myIndex < ((Seperator)item).Parent.Children.Count; myIndex++)
                {
                    if (((Seperator)item).Parent.Children[myIndex] is Directory)
                    {
                        return (Directory)((Seperator)item).Parent.Children[myIndex];
                    }
                }
            }
            return null;
        }
    }
}
