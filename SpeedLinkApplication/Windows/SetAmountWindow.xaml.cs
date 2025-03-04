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
    /// Логика взаимодействия для SetAmountWindow.xaml
    /// </summary>
    public partial class SetAmountWindow : Window
    {
        public int Amount;
        public SetAmountWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var errors = "";
                if (string.IsNullOrWhiteSpace(TBoxAmount.Text)) errors += "Заполните количество\n";

                if (errors == "")
                {
                    Amount = Convert.ToInt32(TBoxAmount.Text);
                    if (Amount <= 0)
                    {
                        MessageBox.Show("Количество товара должно быть положительным числом.",
   "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при сохранении. Проверьте правильность заполнения полей.",
    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
