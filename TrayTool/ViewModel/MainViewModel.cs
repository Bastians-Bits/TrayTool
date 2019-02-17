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
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Seperator> _items;

        public ICommand ButtonAdd { get; private set; }
        public ICommand ButtonRemove { get; private set; }
        public ICommand ButtonBrowserPath { get; private set; }
        public ICommand TreeView { get; private set; }
        public ICommand CbChooser { get; private set; }

        public ObservableCollection<Seperator> Items { get => _items; set => SetProperty(ref _items, value); }

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
            if (TreeView_Selected != null)
            {
                if (TreeView_Selected is Directory)
                {
                    ((Directory)TreeView_Selected).Children.Add(CreateNewInstance(CbAddChooser_Selected, (Directory)TreeView_Selected));
                }
                else
                {
                    // Search for Parent dir and add it there
                    if (TreeView_Selected.Parent != null)
                    {
                        int index = TreeView_Selected.Parent.Children.IndexOf(TreeView_Selected) + 1;

                        // Add it to the parent                   
                        TreeView_Selected.Parent.Children.Insert(index, CreateNewInstance(CbAddChooser_Selected, TreeView_Selected.Parent));
                    }
                    else
                    {
                        int index = Items.IndexOf(TreeView_Selected) + 1;

                        // No parent, the currently selected is a root element
                        Items.Insert(index + 1, CreateNewInstance(CbAddChooser_Selected, null));
                    }
                }
            }
            else
            {
                // No parent dir, we have to create a new root element
                Items.Add(CreateNewInstance(CbAddChooser_Selected, null));
            }
        }

        private void ButtonRemoveClick(object commandParamter)
        {
            if (TreeView_Selected != null)
            {
                //TODO Check for children and warn
                if (TreeView_Selected.Parent != null)
                {
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
                            Items.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
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
    }
}
