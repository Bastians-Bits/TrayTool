using TrayTool.Repository.Model;

namespace TrayTool
{
    public static class ItemExtension
    {
        public static Item UpdateImage(this Item item)
        {
            item.UpdateImage(item.Path);
            return item;
        }

    }
}
