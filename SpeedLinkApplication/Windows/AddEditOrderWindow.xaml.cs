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
using System.Windows.Shapes;

namespace SpeedLinkApplication.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        private Order _order;
        public AddEditOrderWindow(Order order = null)
        {
            InitializeComponent();
            _order = order ?? new Order();
            CBoxService.ItemsSource = App.Context.Service
                .ToList()
                .Where(x => x.IsDeleted == false || x == _order.Service)
                .ToList();

            CBoxClient.ItemsSource = App.Context.Client
                .ToList()
                .Where(x => x.IsDeleted == false || x == _order.Client)
                .ToList();

            CBoxAddress.ItemsSource = App.Context.Address
        .ToList()
        .Where(x => x.IsDeleted == false || x == _order.Address)
        .ToList();

            CBoxTime.ItemsSource = GetTimes();

            if (_order.Id != 0)
            {
                CBoxService.SelectedItem = _order.Service;
                CBoxClient.SelectedItem = _order.Client;
                CBoxAddress.SelectedItem = _order.Address;

                DPickerOrderDate.SelectedDate = _order.OrderDateTime.Date;
                CBoxTime.SelectedItem = _order.OrderDateTime.ToString("HH:mm");

                TBoxDescription.Text = _order.Description;
            }
        }

        private IEnumerable<string> GetTimes()
        {
            for (TimeSpan i = TimeSpan.FromHours(8); i <= TimeSpan.FromHours(20); i = i.Add(TimeSpan.FromMinutes(30)))
            {
                yield return $"{i.Hours:00}:{i.Minutes:00}";
            }
        } 

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var errors = "";
                if (CBoxService.SelectedItem == null) errors += "Выберите услугу\n";
                if (CBoxClient.SelectedItem == null) errors += "Введите номер клиента\n";
                if (CBoxAddress.SelectedItem == null) errors += "Введите адрес\n";
                if (DPickerOrderDate.SelectedDate == null) errors += "Выберите дату\n";
                if (CBoxTime.SelectedItem == null) errors += "Выберите время\n";

                if (errors == "")
                {

                    _order.Service = CBoxService.SelectedItem as Service;
                    _order.Client = CBoxClient.SelectedItem as Client;
                    _order.Address = CBoxAddress.SelectedItem as Address;

                    var time = CBoxTime.SelectedItem.ToString().Split(':');
                    _order.OrderDateTime = DPickerOrderDate.SelectedDate.Value.AddHours(int.Parse(time[0])).AddMinutes(int.Parse(time[1]));
                    _order.Description = TBoxDescription.Text;

                    if (_order.Id == 0)
                    {
                        _order.CreationDateTime = DateTime.Now;
                        _order.OrderStatus = App.Context.OrderStatus.Find(1);
                        _order.User = App.AuthUser;
                        App.Context.Order.Add(_order);
                    }

                    App.Context.SaveChanges();
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при сохранении. Проверьте правильность заполнения полей.",
    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
