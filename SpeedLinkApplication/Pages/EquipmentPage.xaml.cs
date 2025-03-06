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
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        private Equipment equipment => DGridEquip.SelectedItem as Equipment;

        public EquipmentPage()
        {
            InitializeComponent();
            CBoxSort.ItemsSource = new string[] { "Без сортировки", "По названию (возр.)", "По названию (убыв.)" };
            CBoxSort.SelectedIndex = 0;
            UpdateList();
        }
        private void UpdateList()
        {
            List<Equipment> equipments = App.Context.Equipment.ToList();

            string text = TBoxSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
                equipments = equipments.Where(x => x.Name.ToLower().Contains(text) ||
                x.Price.ToString().Contains(text) || x.EquipmentType.Name.ToLower().Contains(text)).ToList();

            switch (CBoxSort.SelectedIndex)
            {
                case 1:
                    equipments = equipments.OrderBy(x => x.Name).ToList();
                    break;
                case 2:
                    equipments = equipments.OrderByDescending(x => x.Name).ToList();
                    break;
                default:
                    break;
            }
            if (ChBoxShowDeleted.IsChecked == false)
                equipments = equipments.Where(x => x.IsDeleted == false).ToList();
            DGridEquip.ItemsSource = equipments;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void CBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void DGridEquip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Equipment selectedItem = DGridEquip.SelectedItem as Equipment;
            var isDeleted = selectedItem?.IsDeleted == true;
            var enabled = equipment != null;
            BtnEdit.IsEnabled = enabled;
            BtnDelete.IsEnabled = enabled;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditEquipmentWindow(equipment);
            if (addWindow.ShowDialog() == true)
            {
                App.Context.SaveChanges();
            }
            UpdateList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данное оборудование?",
"Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                equipment.IsDeleted = true;
                App.Context.SaveChanges();
                UpdateList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Windows.AddEditEquipmentWindow();
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
