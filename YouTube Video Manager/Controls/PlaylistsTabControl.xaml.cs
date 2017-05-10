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
using System.Windows.Threading;
using YouTube_Video_Manager.Helpers;

namespace YouTube_Video_Manager.Controls
{
    /// <summary>
    /// Interaction logic for PlaylistsTabControl.xaml
    /// </summary>
    public partial class PlaylistsTabControl : UserControl
    {

        List<YouTubePlaylist> Playlists = new List<YouTubePlaylist>();
        List<YouTubeVideo> Videos = new List<YouTubeVideo>();

        public PlaylistsTabControl()
        {
            InitializeComponent();
        }

        private async void PlaylistsTabControl_Initialized(object sender, EventArgs e)
        {
            InitializeComponent();

            ErrorGrid.Visibility = Visibility.Collapsed;

            // Set initial visibility of tab
            PlaylistListLoadingSpinner.Visibility = Visibility.Hidden;
            VideoListLoadingSpinner.Visibility = Visibility.Hidden;
            PlaylistPanel.Visibility = Visibility.Visible;
            VideoPanel.Visibility = Visibility.Visible;

            RefreshPlaylistPane();
        }

        public async void RefreshPlaylistPane()
        {

            // Show loading spinner
            PlaylistListLoadingSpinner.Visibility = Visibility.Visible;
            PlaylistPanel.Visibility = Visibility.Hidden;
            VideoPanel.Visibility = Visibility.Hidden;

            // Get list of playlists for the logged in account
            Playlists = await Task.Run(() => YouTubeAPI.GetListOfPlaylists());

            // Display list of playlists
            PlaylistListBox.ItemsSource = Playlists;

            CollectionView view = CollectionViewSource.GetDefaultView(PlaylistListBox.ItemsSource) as CollectionView;
            view.Filter = PlaylistFilter;

            // Hide loading spinner
            PlaylistListLoadingSpinner.Visibility = Visibility.Hidden;
            PlaylistPanel.Visibility = Visibility.Visible;
            VideoPanel.Visibility = Visibility.Visible;

        }

        public async void RefreshVideosPane()
        {

            YouTubePlaylist selectedPlaylist = (PlaylistListBox.SelectedItems[0] as YouTubePlaylist);

            if (selectedPlaylist != null)
            {

                // Show loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoPanel.Visibility = Visibility.Hidden;

                // Get list of playlists for the logged in account
                await Task.Run(() => selectedPlaylist.GetInfo());
                Videos = await Task.Run(() => YouTubeAPI.GetPlaylistVideos(selectedPlaylist));

                // Display list of playlists
                VideoListBox.ItemsSource = Videos;

                CollectionView view = CollectionViewSource.GetDefaultView(VideoListBox.ItemsSource) as CollectionView;
                view.Filter = VideoFilter;

                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Hidden;
                VideoListBox.Visibility = Visibility.Visible;
                VideoPanel.Visibility = Visibility.Visible;

            }

        }

        private void PlaylistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (PlaylistListBox.SelectedItems.Count == 1)
            {
                RefreshVideosPane();
            }
            else if (PlaylistListBox.SelectedItems.Count > 1)
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }

            VideoSearchBox.Text = "";

        }

        private async void VideoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (VideoListBox.SelectedItems.Count == 1)
            {
                YouTubeVideo selectedVideo = VideoListBox.SelectedItems[0] as YouTubeVideo;

                // Get list of playlists for the logged in account
                await Task.Run(() => selectedVideo.GetInfo());

            }
            else if (VideoListBox.SelectedItems.Count > 1)
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }

        }

        private void EditPlaylistMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (PlaylistListBox.SelectedItems.Count == 1)
            {

                EditWindow editWindow = new EditWindow(1, (PlaylistListBox.SelectedItems[0] as YouTubePlaylist).PlaylistID, this);
                editWindow.Show();
            }
            else if (PlaylistListBox.SelectedItems.Count > 1)
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }
        }

        private void EditVideoMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (VideoListBox.SelectedItems.Count == 1)
            {

                EditWindow editWindow = new EditWindow(2, (VideoListBox.SelectedItems[0] as YouTubeVideo).VideoID, this);
                editWindow.Show();
            }
            else if (VideoListBox.SelectedItems.Count > 1)
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hide loading spinner
                VideoListLoadingSpinner.Visibility = Visibility.Visible;
                VideoListBox.Visibility = Visibility.Hidden;
            }
        }

        public void ShowErrorMessage(string ErrorMessage)
        {
            this.ErrorText.Text = ErrorMessage;
            this.ErrorGrid.Visibility = Visibility.Visible;

            DispatcherTimer time = new DispatcherTimer();
            time.Interval = TimeSpan.FromSeconds(5);
            time.Start();
            time.Tick += delegate
            {
                Console.WriteLine(Environment.TickCount);
                this.ErrorText.Text = "";
                this.ErrorGrid.Visibility = Visibility.Hidden;
                time.Stop();
            };
        }

        public bool PlaylistFilter(object playlist)
        {
            if (string.IsNullOrEmpty(PlaylistSearchBox.Text))
            {
                return true;
            }
            else
            {
                return (
                    (playlist as YouTubePlaylist).Title.ToString().IndexOf(PlaylistSearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || (playlist as YouTubePlaylist).Description.ToString().IndexOf(PlaylistSearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        public bool VideoFilter(object video)
        {
            if (string.IsNullOrEmpty(VideoSearchBox.Text))
            {
                return true;
            }
            else
            {
                return (
                    (video as YouTubeVideo).Title.ToString().IndexOf(VideoSearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || (video as YouTubeVideo).Description.ToString().IndexOf(VideoSearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void PlaylistSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PlaylistListBox.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(PlaylistListBox.ItemsSource).Refresh();
            }
        }

        private void VideoSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VideoListBox.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(VideoListBox.ItemsSource).Refresh();
            }
        }
    }
}
