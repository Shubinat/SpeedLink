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
    /// Логика взаимодействия для NonAttachedOrdersPage.xaml
    /// </summary>
    public partial class NonAttachedOrdersPage : Page
    {
        public NonAttachedOrdersPage()
        {
            InitializeComponent();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var orders = App.Context.Order.ToList()
                .Where(x => x.User1 == null && x.OrderStatus.Id == 1).ToList();


            if (!string.IsNullOrEmpty(SearchTextBox.Text))
                orders = orders.Where(x => x.Client.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                x.Address.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                x.Service.Name.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();


            if (DateFromPicker.SelectedDate.HasValue)
                orders = orders.Where(x => x.OrderDateTime.Date >= DateFromPicker.SelectedDate).ToList();

            if (DateToPicker.SelectedDate.HasValue)
                orders = orders.Where(x => x.OrderDateTime.Date <= DateToPicker.SelectedDate).ToList();

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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LViewOrders.SelectedItem is Order order)
            {

                Windows.OrderDetailsWindow window = new Windows.OrderDetailsWindow(order);
                if (window.ShowDialog() == true)
                {
                    UpdateGrid();
                }
            }
        }

        private void LViewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hasSelection = LViewOrders.SelectedItem != null;
            EditButton.IsEnabled = hasSelection;
            AttachButton.IsEnabled = hasSelection;
        }
        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            if (LViewOrders.SelectedItem is Order order)
            {
                order.OrderStatus = App.Context.OrderStatus.Find(2);
                order.User1 = App.AuthUser;
                App.Context.SaveChanges();
                LViewOrders.SelectedItem = null;
                UpdateGrid();
            }
        }
    }
}
