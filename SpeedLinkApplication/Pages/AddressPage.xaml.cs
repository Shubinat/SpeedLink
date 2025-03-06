using SpeedLinkApplication.Entities;
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
    /// Логика взаимодействия для AddressPage.xaml
    /// </summary>
    public partial class AddressPage : Page
    {
        private Address address => DGridAddresses.SelectedItem as Address;
        public AddressPage()
        {
            InitializeComponent();
            CBoxSort.ItemsSource = new string[] { "Без сортировки", "По возрастанию", "По убыванию" };
            CBoxSort.SelectedIndex = 0;
        }

        private void UpdateGrid()
        {
            List<Address> address = App.Context.Address.ToList();

            string text = TBoxSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
                address = address.Where(x => x.FullName.ToLower().Contains(text)).ToList();

            if (ChBoxShowDeleted.IsChecked == false)
                address = address.Where(x => x.IsDeleted == false).ToList();

            switch (CBoxSort.SelectedIndex)
            {
                case 1:
                    address = address.OrderBy(x => x.FullName).ToList();
                    break;
                case 2:
                    address = address.OrderByDescending(x => x.FullName).ToList();
                    break;
                default:
                    break;
            }



            DGridAddresses.ItemsSource = address;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void CBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данный адрес?",
"Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                address.IsDeleted = true;
                App.Context.SaveChanges();
                UpdateGrid();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Windows.AddEditAddressWindow(address);
            if (editWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateGrid();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditAddressWindow();
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateGrid();
        }

        private void ChBoxShowDeleted_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void DGridAddresses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Address selectedItem = DGridAddresses.SelectedItem as Address;
            var isDeleted = selectedItem?.IsDeleted == true;
            var enabled = address != null;
            BtnEdit.IsEnabled = enabled;
            BtnDelete.IsEnabled = enabled && !isDeleted;
        }
    }
}
