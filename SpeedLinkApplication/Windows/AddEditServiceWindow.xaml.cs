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
    /// Логика взаимодействия для AddEditServiceWindow.xaml
    /// </summary>
    public partial class AddEditServiceWindow : Window
    {
        private Service _service;

        public AddEditServiceWindow(Service service = null)
        {
            InitializeComponent();
            if (service != null)
            {
                TBoxName.Text = service.Name;
                TBoxPrice.Text = Convert.ToString(service.Price);
            }

            _service = service;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var err = "";
                if (string.IsNullOrWhiteSpace(TBoxName.Text)) err += "Заполните поле Название\n";
                if (string.IsNullOrWhiteSpace(TBoxPrice.Text)) err += "Заполните поле Цена\n";

                if (err == "")
                {
                    if (_service == null)
                    {
                        try
                        {
                            _service = new Service()
                            {
                                Name = TBoxName.Text,
                                Price = Convert.ToDecimal(TBoxPrice.Text),

                            };
                            App.Context.Service.Add(_service);
                            App.Context.SaveChanges();
                            MessageBox.Show("Услуга сохранена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Произошла ошибка. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        _service.Name = TBoxName.Text;
                        _service.Price = Convert.ToDecimal(TBoxPrice.Text);
                        App.Context.SaveChanges();
                        MessageBox.Show("Услуга сохранена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
