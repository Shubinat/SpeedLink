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
    /// Логика взаимодействия для AddEditAddressWindow.xaml
    /// </summary>
    public partial class AddEditAddressWindow : Window
    {
        private Address _address;
        public AddEditAddressWindow(Address address = null)
        {
            InitializeComponent();
            _address = address;
            if(_address != null)
            {
                TBoxRegion.Text = _address.Region;
                TBoxCity.Text = _address.City;
                TBoxStreet.Text = _address.Street;
                TBoxHome.Text = _address.Home;
                TBoxRoom.Text = _address.Room;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var err = "";
                if (string.IsNullOrWhiteSpace(TBoxRegion.Text)) err += "Заполните регион\n";
                if (string.IsNullOrWhiteSpace(TBoxCity.Text)) err += "Заполните населенный пункт\n";
                if (string.IsNullOrWhiteSpace(TBoxStreet.Text)) err += "Заполните улицу\n";
                if (string.IsNullOrWhiteSpace(TBoxHome.Text)) err += "Выберите номер дома\n";
                if (string.IsNullOrWhiteSpace(TBoxRoom.Text)) err += "Выберите номер квартиры\n";

                if (err == "")
                {
                    if (_address == null)
                    {
                        try
                        {
                            _address = new Address()
                            {
                                Region = TBoxRegion.Text,
                                City = TBoxCity.Text,
                                Street = TBoxStreet.Text,
                                Home = TBoxHome.Text,
                                Room = TBoxRoom.Text,
                            };
                            App.Context.Address.Add(_address);
                            App.Context.SaveChanges();
                            MessageBox.Show("Адрес сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Произошла ошибка. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        _address.Region = TBoxRegion.Text;
                        _address.City = TBoxCity.Text;
                        _address.Street = TBoxStreet.Text;
                        _address.Home = TBoxHome.Text;
                        _address.Room = TBoxRoom.Text;
                        App.Context.SaveChanges();
                        MessageBox.Show("Адрес сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show(err, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch
            {
                _ = MessageBox.Show("Произошла ошибка. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
