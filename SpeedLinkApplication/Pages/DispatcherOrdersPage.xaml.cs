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
    /// Логика взаимодействия для DispatcherOrdersPage.xaml
    /// </summary>
    public partial class DispatcherOrdersPage : Page
    {
        public DispatcherOrdersPage()
        {
            InitializeComponent();
            List<Entities.OrderStatus> orderStatuses = App.Context.OrderStatus.ToList();
            orderStatuses.Insert(0, new Entities.OrderStatus() { Name = "Все" });
            StatusComboBox.ItemsSource = orderStatuses;
            StatusComboBox.SelectedIndex = 0;
        }

        private void UpdateGrid()
        {
            var orders = App.Context.Order.ToList();


            if (!string.IsNullOrEmpty(SearchTextBox.Text))
                orders = orders.Where(x => x.Client.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                x.Address.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                x.Service.Name.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();

            if (StatusComboBox.SelectedIndex != 0)
                orders = orders.Where(x => x.OrderStatus == StatusComboBox.SelectedItem).ToList();

            LViewOrders.ItemsSource = orders.OrderByDescending(x => x.OrderDateTime).ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void DateFromPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void DateToPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void NewRequestButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddEditOrderWindow window = new Windows.AddEditOrderWindow();
            if(window.ShowDialog() == true)
            {
                UpdateGrid();
            }

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(LViewOrders.SelectedItem is Order order)
            {

                Windows.AddEditOrderWindow window = new Windows.AddEditOrderWindow(order);
                if (window.ShowDialog() == true)
                {
                    UpdateGrid();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (LViewOrders.SelectedItem is Order order)
            {
                order.OrderStatus = App.Context.OrderStatus.Find(4);
                order.CompleteDatetime = DateTime.Now;
                App.Context.SaveChanges();
                LViewOrders.SelectedItem = null;
                UpdateGrid();
            }
        }

        private void LViewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hasSelection = LViewOrders.SelectedItem != null;
            EditButton.IsEnabled = hasSelection && (LViewOrders.SelectedItem as Order).OrderStatus.Id == 1;
            CancelButton.IsEnabled = hasSelection && (LViewOrders.SelectedItem as Order).OrderStatus.Id == 1;
        }
    }
}
