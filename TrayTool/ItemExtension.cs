using TrayTool.Repository.Model;

namespace TrayTool
{
    public static class ItemExtension
    {
        public static ItemEntity UpdateImage(this ItemEntity item)
        {
            item.UpdateImage(item.Path);
            return item;
        }

    }
}
