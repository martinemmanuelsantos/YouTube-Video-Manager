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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public async void LoginPage_Load(object sender, RoutedEventArgs e)
        {

            LoadingLabel.Text = "Authenticating user...";

            await Task.Run(async () =>
            {
                try
                {
                    await YouTubeAPI.Authenticate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    System.Environment.Exit(1);
                }
            });

            LoadingLabel.Text = "Authentication successful!";

            this.NavigationService.Navigate(new Uri("Pages/HomePage.xaml", UriKind.Relative));
        }
    }
}
