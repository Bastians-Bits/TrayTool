﻿using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using TrayTool.Extensions;
using TrayTool.Model;

namespace TrayTool.ViewModel
{
    /// <summary>
    /// The main view model, used in the main view
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BaseModel> _items;
        private Seperator _treeView_Selected;

        /// <summary>
        /// Delegater for the Add Button
        /// </summary>
        public ICommand ButtonAdd { get; private set; }
        /// <summary>
        /// Delegator for the Remove Button
        /// </summary>
        public ICommand ButtonRemove { get; private set; }
        /// <summary>
        /// Delegator for the Browse Button in the Item-UserControl
        /// </summary>
        public ICommand ButtonBrowserPath { get; private set; }

        /// <summary>
        /// A list of all items in the application
        /// </summary>
        public ObservableCollection<BaseModel> Items { get => _items; set => SetProperty(ref _items, value); }
        // A list of all argument templates in the application
        public ObservableCollection<ArguementTemplate> ArguementTemplates { get; set; }
        /// <summary>
        /// The currently selected treeview item
        /// </summary>
        public Seperator TreeView_Selected {
            get
            {
                return _treeView_Selected;
            }
            set {
                SetProperty(ref _treeView_Selected, value);
            }
        }
        /// <summary>
        /// The currently selected element in the item chooser (Item, Directory, Seperator)
        /// </summary>
        public int CbAddChooser_Selected { get; set; }

        public MainViewModel()
        {
            ButtonAdd = new DelegateCommand(ButtonAddClick);
            ButtonRemove = new DelegateCommand(ButtonRemoveClick);
            ButtonBrowserPath = new DelegateCommand(ButtonBrowserPathClick);
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
            logger.Trace("ButtonAddClick called");
            if (TreeView_Selected != null)
            {
                if (TreeView_Selected is Directory directory)
                {
                    directory.Children.Add(CreateNewInstance(CbAddChooser_Selected, directory));
                    logger.Debug("Added instance {instance} to the directory {directory}", CbAddChooser_Selected, directory.Name);
                }
                else
                {
                    // Search for Parent dir and add it there
                    if (TreeView_Selected.Parent != null)
                    {
                        int index = TreeView_Selected.Parent.Children.IndexOf(TreeView_Selected) + 1;

                        // Add it to the parent                   
                        TreeView_Selected.Parent.Children.Insert(index, CreateNewInstance(CbAddChooser_Selected, TreeView_Selected.Parent));
                        logger.Debug("Added instance {instance} to the directory {directory}", CbAddChooser_Selected, TreeView_Selected.Parent.Name);
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
                Item item = (Item)TreeView_Selected;
                item.Path = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Create a new Instance of an item, directory, seperator
        /// </summary>
        /// <param name="target">the target type; 0 = Item, 1 = Directory, 2 = Seperator</param>
        /// <param name="parent"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Main Method to handle movement
        /// </summary>
        /// <param name="movement">the movement to handle</param>
        public void Move(Movement movement)
        {
            switch (movement.Moevement)
            {
                case Movement.Direction.UP:
                    movement.Item.MoveUp(Items);
                    break;
                case Movement.Direction.DOWN:
                    movement.Item.MoveDown(Items);
                    break;
                case Movement.Direction.LEFT:
                    movement.Item.MoveLeft(Items);
                    break;
                case Movement.Direction.RIGHT:
                    movement.Item.MoveRight(Items, movement.Index);
                    break;
            }
        }
    }
}
