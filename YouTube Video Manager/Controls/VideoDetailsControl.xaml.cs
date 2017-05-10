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
    /// Interaction logic for ViedoDetailsControl.xaml
    /// </summary>
    public partial class VideoDetailsControl : UserControl
    {
        public VideoDetailsControl()
        {
            InitializeComponent();
        }

        public VideoDetailsControl(YouTubeVideo video)
        {
            InitializeComponent();
        }
    }
}
