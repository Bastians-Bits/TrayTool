using System.ComponentModel.DataAnnotations.Schema;

namespace TrayTool.Repository.Model
{
    public class Seperator : BaseModel
    {
        public Directory Parent { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public bool IsExpanded { get; set; }
        public byte[] Image { get; set; }
        public string ImagePath { get; set; }
    }
}
