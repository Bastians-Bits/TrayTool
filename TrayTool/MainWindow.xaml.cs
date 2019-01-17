using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TrayTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<Item> Items { get; set; }


        public MainWindow()
        {
            Items = new ObservableCollection<Item>()
            {
                new Item() { Name = "Item No 1", Path = "Path No 1" },
                new Item() { Name = "Item No 2", Path = "Path No 2" }
            };

            InitializeComponent();
            DataContext = this;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            string path = tbPath.Text;

            Item item = new Item()
            {
                Name = name,
                Path = path
            };

            Items.Add(item);
        }
    }
}
