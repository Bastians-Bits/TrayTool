namespace TrayTool.Model
{
    public class Seperator : BaseModel
    {
        private string _image;
        private Directory _parent;

        public string Image { get => _image; set => SetProperty(ref _image, value); }

        public Directory Parent { get => _parent; set => SetProperty(ref _parent, value); }

        public Seperator()
        {
            Image = "/TrayTool;component/Resources/Seperator.png";
        }
    }
}
