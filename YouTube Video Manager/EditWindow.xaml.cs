using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YouTube_Video_Manager.Pages;

namespace YouTube_Video_Manager
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {

        public EditWindow()
        {
            InitializeComponent();
        }

        public EditWindow(int page, string ID, UserControl control = null)
        {
            InitializeComponent();

            switch (page)
            {
                case 1:
                    {
                        EditWindowFrame.Content = new EditPlaylistPage(ID, this, control);
                        this.Title = "Edit Playlist Page";
                        break;
                    }
                case 2:
                    {
                        EditWindowFrame.Content = new EditVideoPage(ID, this, control);
                        this.Title = "Edit Video Page";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
