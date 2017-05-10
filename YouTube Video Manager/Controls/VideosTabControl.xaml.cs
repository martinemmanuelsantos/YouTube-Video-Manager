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

namespace YouTube_Video_Manager.Controls
{
    /// <summary>
    /// Interaction logic for VideosTabControl.xaml
    /// </summary>
    public partial class VideosTabControl : UserControl
    {

        List<YouTubeVideo> Videos = new List<YouTubeVideo>();

        public VideosTabControl()
        {
            InitializeComponent();
        }

        private void VideosTabControl_Initialized(object sender, EventArgs e)
        {
            InitializeComponent();

            RefreshPage();
        }

        private void VideoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = VideoListBox.SelectedIndex;

            if (index >= 0)
            {
                Videos[index].GetInfo();
            }

        }

        public async void RefreshPage()
        {

            VideoListLoadingSpinner.Visibility = Visibility.Visible;
            VideoListBox.Visibility = Visibility.Hidden;

            Videos = await Task.Run(() => YouTubeAPI.GetListOfVideos());

            VideoListLoadingSpinner.Visibility = Visibility.Hidden;
            VideoListBox.Visibility = Visibility.Visible;

            VideoListBox.ItemsSource = Videos;

        }
    }
}
