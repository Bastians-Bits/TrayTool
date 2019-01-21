namespace TrayTool.Model
{
    public class BaseModel : AbstractModel
    {
        public Directory _parent;

        protected string _image;
      
        public string Image
        {
            get { return _image; }
            set {
                if (value != _image)
                {
                    _image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public Directory Parent
        {
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPropertyChanged("Parent");
                }
            }
            get
            {
                return _parent;
            }
        }
    }
}
