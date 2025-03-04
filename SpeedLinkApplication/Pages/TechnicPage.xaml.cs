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

namespace SpeedLinkApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для TechnicPage.xaml
    /// </summary>
    public partial class TechnicPage : Page
    {
        public TechnicPage()
        {
            InitializeComponent();
        }

        private void BtnMyOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TechnicOrdersPage());
        }

        private void BtnNonAttachOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NonAttachedOrdersPage());
        }
    }
}
