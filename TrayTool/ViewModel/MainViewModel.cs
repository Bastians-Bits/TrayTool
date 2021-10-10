﻿using Microsoft.EntityFrameworkCore;
using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using TrayTool.Repository;
using TrayTool.Repository.Model;

namespace TrayTool.ViewModel
{
    /// <summary>
    /// The main view model, used in the main view
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public TrayToolDb Context { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BaseModelEntity> _items;
        private SeperatorEntity _treeView_Selected;

        public ICommand ButtonAdd { get; private set; }
        public ICommand ButtonRemove { get; private set; }
        public ICommand ButtonBrowserPath { get; private set; }
        public ObservableCollection<BaseModelEntity> Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
            }
        }
        public SeperatorEntity TreeView_Selected
        {
            get => _treeView_Selected;
            set => SetProperty(ref _treeView_Selected, value);
        }

        public int CbAddChooser_Selected { get; set; }

        public MainViewModel()
        {
            ButtonAdd = new DelegateCommand(ButtonAddClick);
            ButtonRemove = new DelegateCommand(ButtonRemoveClick);
            ButtonBrowserPath = new DelegateCommand(ButtonBrowserPathClick);

            Context = new TrayToolDb();
            Context.Database.Migrate();

            Context.BaseModels.OrderBy(o => o.Order).Include(i => (i as ItemEntity).Arguments).Load();
            _items = Context.BaseModels.Local.ToObservableCollection();
        }

        /// <summary>
        /// Each time a property is changed and the view has to be notified, this method has to be called
        /// </summary>
        /// <typeparam name="T">The type of the set value</typeparam>
        /// <param name="field">A reference to the field</param>
        /// <param name="newValue">The new value of the field</param>
        /// <param name="propertyName">The name of the changed property. Filled by the runtime</param>
        /// <returns></returns>
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

        /// <summary>
        /// Handle the Button Add Event. This adds a new instance (Item, Seperator, Directory) to the tree view
        /// </summary>
        /// <param name="commandParamter">The parameter of the button click</param>
        private void ButtonAddClick(object commandParamter)
        {
            if (TreeView_Selected != null) // Element Select -> Create Below
            {
                if (TreeView_Selected is DirectoryEntity directory) // Element is Directory -> Create as last element in directory
                {
                    SeperatorEntity newEntity = CreateNewInstance(CbAddChooser_Selected, null);
                    newEntity.Order = Context.Directories.Where(w => w.Id == TreeView_Selected.Id).First().Children.Count + 1;
                    Context.Directories.Where(w => w.Id == TreeView_Selected.Id).First().Children.Add(newEntity);
                }
                else // Element is Item -> Create Below
                {
                    if (TreeView_Selected.Parent != null) // Element is part of a directoy -> Add to the same directory
                    {
                        SeperatorEntity newEntity = CreateNewInstance(CbAddChooser_Selected, null);

                        // Update the Entries after
                        Context.Seperators.Where(w => w.Order > TreeView_Selected.Order && w.Parent.Id == TreeView_Selected.Parent.Id).ToList().ForEach(s => s.Order += 1);

                        Context.Seperators.Where(w => w.Id == TreeView_Selected.Id).First().Parent.Children.Add(newEntity);
                    }
                    else // Element is not part of a directory -> Create root below selected
                    {
                        SeperatorEntity newEntity = CreateNewInstance(CbAddChooser_Selected, null);
                        newEntity.Order = TreeView_Selected.Order + 1;

                        // Update the Entries after
                        Context.Seperators.Where(w => w.Order > TreeView_Selected.Order).ToList().ForEach(s => s.Order += 1);

                        Context.Seperators.Add(newEntity);
                    }
                }
            }
            else // No Element Select -> Create Root
            {
                SeperatorEntity newEntity = CreateNewInstance(CbAddChooser_Selected, null);
                newEntity.Order = Context.Seperators.Where(w => w.Parent == null).ToList().Count + 1;
                Context.Seperators.Add(newEntity);
            }

            Context.SaveChanges();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
        }

        /// <summary>
        /// Handle the Buttone Remove Event. This removes the currently selected instance from the tree view
        /// </summary>
        /// <param name="commandParamter">The parameter of the button click</param>
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

            Context.SaveChanges();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
            logger.Trace("ButtonRemoveClick left");
        }

        /// <summary>
        /// Handle the Button Browse Path Event. This calls the OpenFile-Dialog and sets the chosen path into the selected tree view item
        /// </summary>
        /// <param name="commandParamter">The parameter of the button click</param>
        private void ButtonBrowserPathClick(object commandParamter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ItemEntity item = (ItemEntity)TreeView_Selected;
                item.Path = openFileDialog.FileName;
                item.UpdateImage(item.Path);
            }

            Context.SaveChanges();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
        }

        /// <summary>
        /// Create a new Instance of an item, directory, seperator
        /// </summary>
        /// <param name="target">the target type; 0 = Item, 1 = Directory, 2 = Seperator</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public SeperatorEntity CreateNewInstance(int target, DirectoryEntity parent)
        {
            SeperatorEntity instance = null;
            switch (target)
            {
                case 0:
                    instance = new ItemEntity()
                    {
                        Name = "New Item",
                        Parent = parent
                    };
                    instance.UpdateImage(null);
                    break;
                case 1:
                    instance = new DirectoryEntity()
                    {
                        Name = "New Directory",
                        Parent = parent
                    };
                    instance.UpdateImage(null);
                    break;
                case 2:
                    instance = new SeperatorEntity()
                    {
                        Parent = parent
                    };
                    instance.UpdateImage(null);
                    break;
            }
            instance.Id = System.Guid.NewGuid();
            return instance;
        }

        /// <summary>
        /// Main Method to handle movement
        /// </summary>
        /// <param name="movement">the movement to handle</param>
        public void Move(Model.Movement movement)
        {
            switch (movement.Moevement)
            {
                case Model.Movement.Direction.UP:
                    MoveUp(movement.Item);
                    break;
                case Model.Movement.Direction.DOWN:
                    MoveDown(movement.Item);
                    break;
                case Model.Movement.Direction.LEFT:
                    MoveLeft(movement.Item);
                    break;
                case Model.Movement.Direction.RIGHT:
                    MoveRight(movement.Item, movement.Index);
                    break;
            }
        }

        /// <summary>
        /// Move the given item a postion up
        /// </summary>
        /// <param name="item">The item to move</param>
        private void MoveUp(SeperatorEntity item)
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
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is SeperatorEntity seperator)
                {
                    IList<SeperatorEntity> siblings = seperator.Parent.Children;
                    int index = siblings.IndexOf(item) - 1;
                    if (index >= 0)
                    {
                        siblings.RemoveAt(index + 1);
                        siblings.Insert(index, item);
                        //siblings.Move(siblings.IndexOf(item), siblings.IndexOf(item) - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Move the given item a position to the right
        /// </summary>
        /// <param name="item">The item to move</param>
        private void MoveDown(SeperatorEntity item)
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
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is SeperatorEntity seperator)
                {
                    IList<SeperatorEntity> siblings = seperator.Parent.Children;
                    int index = siblings.IndexOf(item) + 1;
                    if (index <= siblings.Count - 1)
                    {
                        siblings.RemoveAt(index - 1);
                        siblings.Insert(index, item);
                        //siblings.Move(siblings.IndexOf(item), siblings.IndexOf(item) + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Move the given item a position to the left
        /// </summary>
        /// <param name="item">The item to move</param>
        private void MoveLeft(SeperatorEntity item)
        {
            if (!IsRoot(item))
            {
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is SeperatorEntity seperator)
                {
                    DirectoryEntity oldParent = seperator.Parent;
                    DirectoryEntity newParent = oldParent.Parent;

                    // Remove old assignment
                    oldParent.Children.Remove(item);
                    seperator.Parent = null;

                    if (newParent != null)
                    {
                        // Create new Assignment
                        newParent.Children.Insert(newParent.Children.IndexOf(oldParent), item);
                        seperator.Parent = newParent;
                    }
                    else
                    {
                        // Means here, we have a new root node
                        Items.Insert(Items.IndexOf(oldParent), item);
                    }
                }
            }
        }

        /// <summary>
        /// Move the given item a position to the right
        /// </summary>
        /// <param name="item">The item to move</param>
        /// <param name="index">The index of the new parent (WIP)</param>
        private void MoveRight(SeperatorEntity item, int? index = null)
        {
            DirectoryEntity newParent = NearestDirectory(item);
            
            if (newParent != null)
            {
                // Open the new parent directory for better visibility
                newParent.IsExpanded = true;
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is SeperatorEntity seperator)
                {
                    if (IsRoot(item))
                    {
                        Items.Remove(item);
                    }
                    else
                    {
                        seperator.Parent.Children.Remove(item);
                    }
                    seperator.Parent = newParent;
                }
                newParent.Children.Insert(0, item);
            }
        }

        /// <summary>
        /// Check if a given item is a root element. A root element is defined by not having a parent
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True, if is a root element, otherwise false</returns>
        public bool IsRoot(BaseModelEntity item)
        {
            if (item is SeperatorEntity seperator)
            {
                if (seperator.Parent == null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the nearest directory to a given item
        /// </summary>
        /// <param name="item">The item to search by</param>
        /// <returns>The directory, null if none has been found</returns>
        public DirectoryEntity NearestDirectory(SeperatorEntity item)
        {
            int myIndex;
            if (IsRoot(item))
            {
                // Get the next directory from the list
                myIndex = Items.IndexOf(item);
                if (item is DirectoryEntity) myIndex++; 
                for (; myIndex < Items.Count; myIndex++)
                {
                    if (Items[myIndex] is DirectoryEntity directory)
                    {
                        return directory;
                    }
                }
            }
            else
            {
                // Get the next directory from the siblings
                myIndex = ((SeperatorEntity)item).Parent.Children.IndexOf(item);
                if (item is DirectoryEntity) myIndex++;
                for (; myIndex < ((SeperatorEntity)item).Parent.Children.Count; myIndex++)
                {
                    if (((SeperatorEntity)item).Parent.Children[myIndex] is DirectoryEntity directory)
                    {
                        return directory;
                    }
                }
            }
            return null;
        }
    }
}
