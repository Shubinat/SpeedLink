using SpeedLinkApplication.Entities;
using SpeedLinkApplication.Windows;
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
    /// Логика взаимодействия для AdminOrdersPage.xaml
    /// </summary>
    public partial class AdminOrdersPage : Page
    {
        public AdminOrdersPage()
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

            if (DateFromPicker.SelectedDate.HasValue)
                orders = orders.Where(x => x.OrderDateTime.Date >= DateFromPicker.SelectedDate).ToList();

            if (DateToPicker.SelectedDate.HasValue)
                orders = orders.Where(x => x.OrderDateTime.Date <= DateToPicker.SelectedDate).ToList();

            ChartButton.IsEnabled = orders.Count > 0;
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

        private void LViewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hasSelection = LViewOrders.SelectedItem != null;
            DetailsButton.IsEnabled = hasSelection;
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if(LViewOrders.SelectedItem is Order order)
            {
                Windows.OrderDetailsWindow window = new Windows.OrderDetailsWindow(order);
                if (window.ShowDialog() == true)
                {
                    UpdateGrid();
                }
            }
        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            var orders = LViewOrders.ItemsSource as List<Order>;
            if(orders.Count > 0)
            {
                OrderChartWindow window = new OrderChartWindow(orders);
                if(window.ShowDialog() == true) {

                }
            }
        }
    }
}
