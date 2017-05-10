using Google.Apis.YouTube.v3.Data;
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
using YouTube_Video_Manager.Controls;

namespace YouTube_Video_Manager.Pages
{
    /// <summary>
    /// Interaction logic for EditVideoPage.xaml
    /// </summary>
    public partial class EditVideoPage : Page
    {

        Window EditVideoWindow;
        UserControl UserControl;

        YouTubeVideo YTvideo;
        String ErrorMessage = "";

        int TitleCharLimit = 100;
        int DescriptionCharLimit = 5000;
        int TagsCharLimit = 500;

        public EditVideoPage()
        {
            InitializeComponent();
        }

        public EditVideoPage(string ID, Window window, UserControl control = null)
        {
            InitializeComponent();

            this.EditVideoWindow = window;
            this.UserControl = control;

            YTvideo = new YouTubeVideo(ID);
        }

        private async void EditVideoPage_Loaded(object sender, RoutedEventArgs e)
        {

            ErrorGrid.Visibility = Visibility.Collapsed;

            LoadingSpinner.Visibility = Visibility.Visible;
            ContentsDockPanel.Opacity = 0.3;

            bool success = false;

            await Task.Run(() => {
                try
                {
                    YTvideo.GetInfo();

                    if (YTvideo.ChannelID == YouTubeAPI.channelID)
                    {
                        success = true;
                    } else
                    {
                        ErrorMessage = "Error: Could not download video metadata";
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error: " + ex.Message;
                    success = false;
                }

            });

            ContentsDockPanel.Opacity = 1.0;
            LoadingSpinner.Visibility = Visibility.Hidden;

            if (success == false)
            {
                if (UserControl is PlaylistsTabControl)
                {
                    ((PlaylistsTabControl)UserControl).ShowErrorMessage(ErrorMessage);
                }

                if (UserControl is VideosTabControl)
                {
                    //((VideosTabControl)UserControl).RefreshVideosPane();
                }
                this.EditVideoWindow.Close();
            }

            this.DataContext = YTvideo;

            //DescriptionCharLimitText.Text = string.Format("({0}/5000)", DescriptionTextBox.Text.Length);
            //TagsCharLimitText.Text = string.Format("({0}/500)", TagsTextBox.Text.Length);

            ContentsDockPanel.Opacity = 1.0;
            LoadingSpinner.Visibility = Visibility.Hidden;

        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TitleCharLimitText.Text = string.Format("({0}/{1})", TitleTextBox.Text.Length, TitleCharLimit);

            if (TitleTextBox.Text.Length > TitleCharLimit)
            {
                TitleCharLimitText.Foreground = new SolidColorBrush(Colors.Red);
            } else
            {
                TitleCharLimitText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionCharLimitText.Text = string.Format("({0}/{1})", DescriptionTextBox.Text.Length, DescriptionCharLimit);

            if (DescriptionTextBox.Text.Length > DescriptionCharLimit)
            {
                DescriptionCharLimitText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                DescriptionCharLimitText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TagsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TagsCharLimitText.Text = string.Format("({0}/{1})", TagsTextBox.Text.Length, TagsCharLimit);

            if (TagsTextBox.Text.Length > TagsCharLimit)
            {
                TagsCharLimitText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                TagsCharLimitText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (TitleTextBox.Text.Length > TitleCharLimit)
            {
                ShowErrorMessage(string.Format("Error: Title exceeds {0} character limit", TitleCharLimit));
                return;
            }

            if (DescriptionTextBox.Text.Length > DescriptionCharLimit)
            {
                ShowErrorMessage(string.Format("Error: Description exceeds {0} character limit", DescriptionCharLimit));
                return;
            }

            if (TagsTextBox.Text.Length > TagsCharLimit)
            {
                ShowErrorMessage(string.Format("Error: Tags exceed {0} character limit", TagsCharLimit));
                return;
            }

            Video vid = new Video();

            vid.Snippet = new VideoSnippet();
            vid.Snippet.Title = TitleTextBox.Text;
            vid.Snippet.Description = DescriptionTextBox.Text;
            vid.Snippet.CategoryId = YTvideo.CategoryID;
            String[] tags = TagsTextBox.Text.Split(',');
            vid.Snippet.Tags = tags;
            vid.Id = YTvideo.VideoID;

            LoadingSpinner.Visibility = Visibility.Visible;
            ContentsDockPanel.Opacity = 0.3;

            bool success = false;

            await Task.Run(() => {
                try
                {
                    YouTubeAPI.YTservice.Videos.Update(vid, "id,snippet").Execute();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ErrorMessage = "Error: " + ex.Message;
                    success = false;
                }

            });

            ContentsDockPanel.Opacity = 1.0;
            LoadingSpinner.Visibility = Visibility.Hidden;

            if (success == true)
            {
                if (UserControl is PlaylistsTabControl)
                {
                    ((PlaylistsTabControl)UserControl).RefreshVideosPane();
                }

                if (UserControl is VideosTabControl)
                {
                    //((VideosTabControl)UserControl).RefreshVideosPane();
                }

                this.EditVideoWindow.Close();
            }
            else
            {
                ShowErrorMessage(ErrorMessage);
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

    }
}
