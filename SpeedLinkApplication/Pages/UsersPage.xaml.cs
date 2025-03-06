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
using SpeedLinkApplication.Entities;

namespace SpeedLinkApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private User user => DGridUsers.SelectedItem as User;

        public UsersPage()
        {
            InitializeComponent();
            CBoxSort.ItemsSource = new string[] { "Без сортировки", "По ФИО (возр.)", "По ФИО (убыв.)" };
            CBoxSort.SelectedIndex = 0;
            UpdateList();
        }
        private void UpdateList()
        {
            List<User> users = App.Context.User.ToList();

            string text = TBoxSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
                users = users.Where(x => x.FullName.ToLower().Contains(text) || x.Login.ToLower().Contains(text) ||
                x.PhoneNumber.ToString().Contains(text) || x.Email.ToLower().Contains(text) 
                || x.Role.Name.ToLower().Contains(text)).ToList();

            switch (CBoxSort.SelectedIndex)
            {
                case 1:
                    users = users.OrderBy(x => x.FullName).ToList();
                    break;
                case 2:
                    users = users.OrderByDescending(x => x.FullName).ToList();
                    break;
                default:
                    break;
            }
            if (ChBoxShowDeleted.IsChecked == false)
                users = users.Where(x => x.IsDeleted == false).ToList();
            DGridUsers.ItemsSource = users;
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void CBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void DGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User selectedItem = DGridUsers.SelectedItem as User;
            var isDeleted = selectedItem?.IsDeleted == true;
            var enabled = user != null;
            BtnEdit.IsEnabled = enabled;
            BtnDelete.IsEnabled = enabled;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditUserWindow(user);
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данного пользователя?",
"Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                user.IsDeleted = true;
                App.Context.SaveChanges();
                UpdateList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditUserWindow();
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateList();
        }

        private void ChBoxShowDeleted_Click(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
    }
}
