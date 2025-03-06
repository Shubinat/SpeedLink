using SpeedLinkApplication.Entities;
using SpeedLinkApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private Client client => DGridClients.SelectedItem as Client;
        public ClientPage()
        {
            InitializeComponent();
            List<ClientType> clientTypes = App.Context.ClientType.ToList();
            clientTypes.Insert(0,new ClientType() { Name = "Все"});
            CBoxClientType.ItemsSource = clientTypes;
            CBoxClientType.SelectedIndex = 0;
            CBoxSort.ItemsSource = new string[] { "Без сортировки", "По возрастанию", "По убыванию" };
            CBoxSort.SelectedIndex = 0;
        }
        private void UpdateGrid()
        {
            List<Client> clients = App.Context.Client.ToList();

            string text = TBoxSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
                clients = clients.Where(x => x.FullName.ToLower().Contains(text) ||
                x.PhoneNumber.ToLower().Contains(text) ||
                x.Email.ToLower().Contains(text)).ToList();

            if (ChBoxShowDeleted.IsChecked == false)
                clients = clients.Where(x => x.IsDeleted == false).ToList();

            if(CBoxClientType.SelectedIndex != 0)
                clients = clients.Where(x => x.ClientType == CBoxClientType.SelectedItem).ToList();

            switch (CBoxSort.SelectedIndex)
            {
                case 1:
                    clients = clients.OrderBy(x => x.FullName).ToList();
                    break;
                case 2:
                    clients = clients.OrderByDescending(x => x.FullName).ToList();
                    break;
                default:
                    break;
            }

            DGridClients.ItemsSource = clients; 
        }
        private void CBoxClientType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }
        private void CBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void ChBoxShowDeleted_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditClientWindow();
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateGrid();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditClientWindow(client);
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateGrid();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данного клиента?",
"Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                client.IsDeleted = true;
                App.Context.SaveChanges();
                UpdateGrid();
            }
        }

        private void DGridClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client selectedItem = DGridClients.SelectedItem as Client;
            var isDeleted = selectedItem?.IsDeleted == true;
            var enabled = client != null;
            BtnEdit.IsEnabled = enabled;
            BtnDelete.IsEnabled = enabled && !isDeleted;
        }
    }
}
