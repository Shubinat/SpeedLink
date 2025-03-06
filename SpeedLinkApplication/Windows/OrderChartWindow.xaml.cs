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
    /// Логика взаимодействия для OrderChartWindow.xaml
    /// </summary>
    public partial class OrderChartWindow : Window
    {
        public OrderChartWindow(List<Order> orders)
        {
            InitializeComponent();

            var maxDate = orders.Max(x => x.OrderDateTime.Date);
            var minDate = orders.Min(x => x.OrderDateTime.Date);
            Title = $"График за период с {minDate:dd.MM.yyyy} по {maxDate:dd.MM.yyyy}";

            var series = AnalChart.Series.Add("Количество заявок");

            var data = orders.GroupBy(x => x.OrderDateTime.Date).OrderBy(x => x.Key).ToList();
            var xAxis = data.Select(x => x.Key).ToArray();
            var yAxis = data.Select(x => x.Count()).ToArray();
            series.Points.DataBindXY(xAxis, yAxis);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if(dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(WinFormHost, "");
                DialogResult = true;
            }
        }
    }
}
