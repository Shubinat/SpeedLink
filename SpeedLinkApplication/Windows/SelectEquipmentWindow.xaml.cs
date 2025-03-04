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
    /// Логика взаимодействия для SelectEquipmentWindow.xaml
    /// </summary>
    public partial class SelectEquipmentWindow : Window
    {
        public OrderEquipment Equipment;
        private List<OrderEquipment> _orderEq;

        public SelectEquipmentWindow(List<OrderEquipment> orderEq)
        {
            InitializeComponent();
            _orderEq = orderEq;
            List<EquipmentType> categories = App.Context.EquipmentType.ToList();
            categories.Insert(0, new EquipmentType() { Name = "Все" });
            CBoxCategory.ItemsSource = categories;
            CBoxCategory.SelectedIndex = 0;
           
        }
        public void UpdateList()
        {
            var list = App.Context.Equipment.ToList();

            list = list.Where(x => _orderEq.FirstOrDefault(y => y.Equipment == x) == null).ToList();

            if (!string.IsNullOrEmpty(TBoxSearch.Text))
                list = list.Where(x => x.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (CBoxCategory.SelectedIndex != 0)
                list = list.Where(x => x.EquipmentType == CBoxCategory.SelectedItem).ToList();

            DGridEquipment.ItemsSource = list;
        }
        private void CBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (DGridEquipment.SelectedItem is Equipment eq)
            {
                SetAmountWindow window = new SetAmountWindow();
                if (window.ShowDialog() == true)
                {
                    Equipment = new OrderEquipment()
                    {
                        Amount = window.Amount,
                        Equipment = eq
                    };
                    DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Товар не выбран.",
     "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
