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
using SpeedLinkApplication.Entities;

namespace SpeedLinkApplication.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        private User _user;

        public AddEditUserWindow(User user = null)
        {
            InitializeComponent();
            CBoxRole.ItemsSource = App.Context.Role.ToList();
            if (user != null)
            {
                TBoxName.Text = user.Name;
                TBoxSurname.Text = user.Surname;
                TBoxPatronymic.Text = user.Patronymic;
                TBoxPassword.Text = user.Password;
                TBoxLogin.Text = user.Login;
                CBoxRole.Text = user.Role.Name;
                TBoxEmail.Text = user.Email;
                TBoxPhone.Text = user.PhoneNumber;

            }

            _user = user;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var err = "";
                if (string.IsNullOrWhiteSpace(TBoxName.Text)) err += "Заполните поле Имя\n";
                if (string.IsNullOrWhiteSpace(TBoxSurname.Text)) err += "Заполните поле Фамилия\n";
                if (string.IsNullOrWhiteSpace(TBoxPatronymic.Text)) err += "Заполните поле Отчество\n";
                if (string.IsNullOrWhiteSpace(TBoxEmail.Text)) err += "Заполните поле Email\n";
                if (string.IsNullOrWhiteSpace(TBoxPhone.Text)) err += "Заполните поле Телефон\n";
                if (string.IsNullOrWhiteSpace(CBoxRole.Text)) err += "Выберите должность\n";
                if (string.IsNullOrWhiteSpace(TBoxLogin.Text)) err += "Заполните поле Логин\n";
                if (string.IsNullOrWhiteSpace(TBoxPassword.Text)) err += "Заполните поле Пароль\n";

                if (err == "")
                {
                    if (_user == null)
                    {
                        try
                        {
                            _user = new User()
                            {
                                Name = TBoxName.Text,
                                Surname = TBoxSurname.Text,
                                Patronymic = TBoxPatronymic.Text,
                                Password = TBoxPassword.Text,
                                Login = TBoxLogin.Text,
                                PhoneNumber = TBoxPhone.Text,
                                Email = TBoxEmail.Text,
                                RoleId = (CBoxRole.SelectedItem as Role).Id,

                            };
                            App.Context.User.Add(_user);
                            App.Context.SaveChanges();
                            MessageBox.Show("Пользователь сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Произошла ошибка. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        _user.Name = TBoxName.Text;
                        _user.Surname = TBoxSurname.Text;
                        _user.Patronymic = TBoxPatronymic.Text;
                        _user.Login = TBoxLogin.Text;
                        _user.Password = TBoxPassword.Text;
                        _user.PhoneNumber = TBoxPhone.Text;
                        _user.Email = TBoxEmail.Text;
                        _user.RoleId = (CBoxRole.SelectedItem as Role).Id;
                        App.Context.SaveChanges();
                        MessageBox.Show("Пользователь сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
