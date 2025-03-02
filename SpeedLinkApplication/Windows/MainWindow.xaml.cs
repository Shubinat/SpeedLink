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

namespace SpeedLinkApplication.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new Pages.AuthPage());
        }

        private void FrameMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrameMain.Content is Pages.AuthPage)
            {
                BtnBack.Visibility = Visibility.Collapsed;
                GridRowFooter.Height = GridRowHeader.Height = 
                    new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                BtnBack.Visibility = Visibility.Visible;
                GridRowFooter.Height = GridRowHeader.Height =
    new GridLength(50, GridUnitType.Pixel);
            }
            TextBlockHeader.Text = "SpeedLink - " + (FrameMain.Content as Page).Title;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameMain.CanGoBack)
                FrameMain.GoBack();
        }
    }
}
