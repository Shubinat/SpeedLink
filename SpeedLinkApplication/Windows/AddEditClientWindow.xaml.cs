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
    /// Логика взаимодействия для AddEditClientWindow.xaml
    /// </summary>
    public partial class AddEditClientWindow : Window
    {
        private Client _client;
        public AddEditClientWindow(Client client = null)
        {
            InitializeComponent();
            _client = client;

            CBoxClientType.ItemsSource = App.Context.ClientType.ToList();
            if (client != null)
            {
                TBoxName.Text = _client.FullName;
                TBoxEmail.Text = _client.Email;
                TBoxPhoneNumber.Text = _client.PhoneNumber;
                CBoxClientType.SelectedItem = _client.ClientType;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var err = "";
                if (string.IsNullOrWhiteSpace(TBoxName.Text)) err += "Заполните поле Клиент\n";
                if (string.IsNullOrWhiteSpace(TBoxEmail.Text)) err += "Заполните поле Email\n";
                if (string.IsNullOrWhiteSpace(TBoxPhoneNumber.Text)) err += "Заполните поле Номер телефона\n";
                if (CBoxClientType.SelectedIndex == -1) err += "Выберите Тип клиента\n";

                if (err == "")
                {
                    if (_client == null)
                    {
                        try
                        {
                            _client = new Client()
                            {
                                FullName = TBoxName.Text,
                                Email = TBoxEmail.Text,
                                PhoneNumber = TBoxPhoneNumber.Text,
                                ClientType = (CBoxClientType.SelectedItem as ClientType),

                            };
                            App.Context.Client.Add(_client);
                            App.Context.SaveChanges();
                            MessageBox.Show("Клиент сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Произошла ошибка. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        _client.FullName = TBoxName.Text;
                        _client.Email = TBoxEmail.Text;
                        _client.PhoneNumber = TBoxPhoneNumber.Text;
                        _client.ClientType = (CBoxClientType.SelectedItem as ClientType);
                        App.Context.SaveChanges();
                        MessageBox.Show("Клиент сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
