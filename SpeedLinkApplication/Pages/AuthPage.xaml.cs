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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginTextBox.Text) && !string.IsNullOrEmpty(PasswordBox.Password))
            {
                if (App.Context.User.ToList()
                    .FirstOrDefault(x => x.Login == LoginTextBox.Text && x.Password == PasswordBox.Password) is User user)
                {
                    App.AuthUser = user;
                    switch (App.AuthUser.RoleId)
                    {
                        case 1: // Диспетчер
                            NavigationService.Navigate(new DispatcherPage());
                            break;
                        case 2: // Техник
                            NavigationService.Navigate(new TechnicPage());
                            break;
                        case 3: // Администратор
                            NavigationService.Navigate(new AdminPage());
                            break;
                        default:
                            MessageBox.Show("Для данной роли не предусмотрен функционал", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Заполните оба поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
