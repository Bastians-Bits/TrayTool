using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrayTool.Model;

namespace TrayTool.Extensions
{
    public static class BaseModelExtension
    {
        public static void MoveUp(this BaseModel item, ObservableCollection<BaseModel> items)
        {
            if (item.IsRoot())
            {
                int index = items.IndexOf(item) - 1;
                if (index >= 0)
                {
                    items.Move(items.IndexOf(item), items.IndexOf(item) - 1);
                }
            }
            else
            {
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is Seperator seperator)
                {
                    ObservableCollection<BaseModel> siblings = seperator.Parent.Children;
                    int index = siblings.IndexOf(item) - 1;
                    if (index >= 0)
                    {
                        siblings.Move(siblings.IndexOf(item), siblings.IndexOf(item) - 1);
                    }
                }
            }
        }

        public static void MoveDown(this BaseModel item, ObservableCollection<BaseModel> items)
        {
            if (item.IsRoot())
            {
                int index = items.IndexOf(item) + 1;
                if (index <= items.Count - 1)
                {
                    items.Move(items.IndexOf(item), items.IndexOf(item) + 1);
                }
            }
            else
            {
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is Seperator seperator)
                {
                    ObservableCollection<BaseModel> siblings = seperator.Parent.Children;
                    int index = siblings.IndexOf(item) + 1;
                    if (index <= siblings.Count - 1)
                    {
                        siblings.Move(siblings.IndexOf(item), siblings.IndexOf(item) + 1);
                    }
                }
            }
        }

        public static void MoveRight(this BaseModel item, ObservableCollection<BaseModel> items, int index)
        {
            Directory newParent = item.NearestDirectory(items);

            if (newParent != null)
            {
                // Open the new parent directory for better visibility
                newParent.IsExpanded = true;
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is Seperator seperator)
                {
                    if (item.IsRoot())
                    {
                        items.Remove(item);
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

        public static void MoveLeft(this BaseModel item, ObservableCollection<BaseModel> items)
        {
            if (!item.IsRoot())
            {
                // In the current hierarchy, this makes no sense, BaseModel is always a Seperator, but this way we are future-proof
                if (item is Seperator seperator)
                {
                    Directory oldParent = seperator.Parent;
                    Directory newParent = oldParent.Parent;

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
                        items.Insert(items.IndexOf(oldParent), item);
                    }
                }
            }
        }

        /// <summary>
        /// Check if a given item is a root element. A root element is defined by not having a parent
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True, if is a root element, otherwise false</returns>
        public static bool IsRoot(this BaseModel item)
        {
            if (item is Seperator seperator)
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
        public static Directory NearestDirectory(this BaseModel item, ObservableCollection<BaseModel> items)
        {
            int myIndex;
            if (item.IsRoot())
            {
                // Get the next directory from the list
                myIndex = items.IndexOf(item);
                if (item is Directory) myIndex++;
                for (; myIndex < items.Count; myIndex++)
                {
                    if (items[myIndex] is Directory directory)
                    {
                        return directory;
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
                    if (((Seperator)item).Parent.Children[myIndex] is Directory directory)
                    {
                        return directory;
                    }
                }
            }
            return null;
        }
    }
}
