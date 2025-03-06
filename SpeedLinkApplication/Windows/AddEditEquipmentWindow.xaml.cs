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
    /// Логика взаимодействия для AddEditEquipmentWindow.xaml
    /// </summary>
    public partial class AddEditEquipmentWindow : Window
    {
        private Equipment _eq;

        public AddEditEquipmentWindow(Equipment eq = null)
        {
            InitializeComponent();
            CBoxEqType.ItemsSource = App.Context.EquipmentType.ToList();
            if (eq != null)
            {
                TBoxName.Text = eq.Name;
                TBoxPrice.Text = Convert.ToString(eq.Price);
                CBoxEqType.Text = eq.EquipmentType.Name;
            }

            _eq = eq;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var err = "";
                if (string.IsNullOrWhiteSpace(TBoxName.Text)) err += "Заполните поле Название\n";
                if (string.IsNullOrWhiteSpace(TBoxPrice.Text)) err += "Заполните поле Цена\n";
                if (string.IsNullOrWhiteSpace(CBoxEqType.Text)) err += "Выберите тип оборудования\n";

                if (err == "")
                {
                    if (_eq == null)
                    {
                        try
                        {
                            _eq = new Equipment()
                            {
                                Name = TBoxName.Text,
                                Price = Convert.ToDecimal(TBoxPrice.Text),
                                EquipmentTypeId = (CBoxEqType.SelectedItem as EquipmentType).Id,

                            };
                            App.Context.Equipment.Add(_eq);
                            App.Context.SaveChanges();
                            MessageBox.Show("Оборудование сохранено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Произошла ошибка. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        _eq.Name = TBoxName.Text;
                        _eq.Price = Convert.ToDecimal(TBoxPrice.Text);
                        _eq.EquipmentTypeId = (CBoxEqType.SelectedItem as EquipmentType).Id;
                        App.Context.SaveChanges();
                        MessageBox.Show("Оборудование сохранено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
