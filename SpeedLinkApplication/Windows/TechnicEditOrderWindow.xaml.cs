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
    /// Логика взаимодействия для TechnicEditOrderWindow.xaml
    /// </summary>
    public partial class TechnicEditOrderWindow : Window
    {
        private List<OrderEquipment> _orderEquipment;
        private Order _order;

        public TechnicEditOrderWindow(Order order)
        {
            InitializeComponent();
            SPanelBindable.DataContext = _order = order;
            TBoxDescription.Text = order.Description;
            LViewOrderEquipment.ItemsSource = _orderEquipment = order.OrderEquipment.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _order.Description = TBoxDescription.Text;
            
            var removeEq = _order.OrderEquipment.ToList().Where(y => !_orderEquipment.ToList().Contains(y));
            foreach (var item in removeEq)
                App.Context.OrderEquipment.Remove(item);

            var newEq = _orderEquipment.ToList().Where(x => x.Id == 0).ToList();
            foreach (var item in newEq)
            {
                item.Order = _order;
                App.Context.OrderEquipment.Add(item);
            }

            App.Context.SaveChanges();
            DialogResult = true;
        }

        private void BtnAddEq_Click(object sender, RoutedEventArgs e)
        {
            SelectEquipmentWindow window = new SelectEquipmentWindow(_orderEquipment.ToList());
            if (window.ShowDialog() == true)
            {
                _orderEquipment.Add(window.Equipment);
                UpdateGrid();
            }
        }

        private void BtnRemoveEq_Click(object sender, RoutedEventArgs e)
        {
            if (LViewOrderEquipment.SelectedItem is OrderEquipment orderEq)
            {
                _orderEquipment.Remove(orderEq);
                UpdateGrid();
            }
        }
        private void UpdateGrid()
        {
            LViewOrderEquipment.ItemsSource = _orderEquipment.ToList();
        }
    }
}
