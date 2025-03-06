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
using SpeedLinkApplication.Entities;

namespace SpeedLinkApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private Service service => DGridServices.SelectedItem as Service;

        public ServicePage()
        {
            InitializeComponent();
            CBoxSort.ItemsSource = new string[] { "Без сортировки", "По названию (возр.)", "По названию (убыв.)" };
            CBoxSort.SelectedIndex = 0;
            UpdateList();
        }
        private void UpdateList()
        {
            List<Service> services = App.Context.Service.ToList();

            string text = TBoxSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
                services = services.Where(x => x.Name.ToLower().Contains(text) || x.Price.ToString().Contains(text)).ToList();

            switch (CBoxSort.SelectedIndex)
            {
                case 1:
                    services = services.OrderBy(x => x.Name).ToList();
                    break;
                case 2:
                    services = services.OrderByDescending(x => x.Name).ToList();
                    break;
                default:
                    break;
            }
            if (ChBoxShowDeleted.IsChecked == false)
                services = services.Where(x => x.IsDeleted == false).ToList();
            DGridServices.ItemsSource = services;
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void CBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void DGridServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Service selectedItem = DGridServices.SelectedItem as Service;
            var isDeleted = selectedItem?.IsDeleted == true;
            var enabled = service != null;
            BtnEdit.IsEnabled = enabled;
            BtnDelete.IsEnabled = enabled && !isDeleted;
        }

        private void ChBoxShowDeleted_Click(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditServiceWindow(service);
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данную услугу?",
"Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                service.IsDeleted = true;
                App.Context.SaveChanges();
                UpdateList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditServiceWindow();
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateList();
        }
    }
}
