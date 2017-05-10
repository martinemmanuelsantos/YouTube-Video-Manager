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
using System.Windows.Navigation;
using System.Windows.Shapes;
using YouTube_Video_Manager.Helpers;

namespace YouTube_Video_Manager.Pages
{
    /// <summary>
    /// Interaction logic for EditPlaylistPage.xaml
    /// </summary>
    public partial class EditPlaylistPage : Page
    {

        Window EditPlaylistWindow;
        UserControl UserControl;
        YouTubePlaylist YTplaylist;

        public EditPlaylistPage()
        {
            InitializeComponent();
        }

        public EditPlaylistPage(string ID, Window window, UserControl control = null)
        {
            InitializeComponent();

            this.EditPlaylistWindow = window;
            this.UserControl = control;

            YTplaylist = new YouTubePlaylist(ID);
            YTplaylist.GetInfo();

            this.DataContext = YTplaylist;

            DescriptionCharLimitText.Text = string.Format("({0}/5000)", DescriptionTextBox.Text.Length);
            TagsCharLimitText.Text = string.Format("({0}/500)", TagsTextBox.Text.Length);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.EditPlaylistWindow.Close();
        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TitleCharLimitText.Text = string.Format("({0}/100)", TitleTextBox.Text.Length);
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionCharLimitText.Text = string.Format("({0}/5000)", DescriptionTextBox.Text.Length);
        }

        private void TagsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TagsCharLimitText.Text = string.Format("({0}/500)", TagsTextBox.Text.Length);
        }
    }
}
