using SpeedLinkApplication.Entities;
using SpeedLinkApplication.Windows;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace SpeedLinkApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminOrdersPage.xaml
    /// </summary>
    public partial class AdminOrdersPage : Page
    {
        public AdminOrdersPage()
        {
            InitializeComponent();
            List<Entities.OrderStatus> orderStatuses = App.Context.OrderStatus.ToList();
            orderStatuses.Insert(0, new Entities.OrderStatus() { Name = "Все" });
            StatusComboBox.ItemsSource = orderStatuses;
            StatusComboBox.SelectedIndex = 0;
        }

        private void UpdateGrid()
        {
            var orders = App.Context.Order.ToList();


            if (!string.IsNullOrEmpty(SearchTextBox.Text))
                orders = orders.Where(x => x.Client.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                x.Address.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                x.Service.Name.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();

            if (StatusComboBox.SelectedIndex != 0)
                orders = orders.Where(x => x.OrderStatus == StatusComboBox.SelectedItem).ToList();

            if (DateFromPicker.SelectedDate.HasValue)
                orders = orders.Where(x => x.OrderDateTime.Date >= DateFromPicker.SelectedDate).ToList();

            if (DateToPicker.SelectedDate.HasValue)
                orders = orders.Where(x => x.OrderDateTime.Date <= DateToPicker.SelectedDate).ToList();

            ChartButton.IsEnabled = orders.Count > 0;
            ExcelButton.IsEnabled = orders.Count > 0;
            LViewOrders.ItemsSource = orders.OrderByDescending(x => x.OrderDateTime).ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void DateFromPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void DateToPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void LViewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hasSelection = LViewOrders.SelectedItem != null;
            DetailsButton.IsEnabled = hasSelection;
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if(LViewOrders.SelectedItem is Order order)
            {
                Windows.OrderDetailsWindow window = new Windows.OrderDetailsWindow(order);
                if (window.ShowDialog() == true)
                {
                    UpdateGrid();
                }
            }
        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            var orders = LViewOrders.ItemsSource as List<Order>;
            if(orders.Count > 0)
            {
                OrderChartWindow window = new OrderChartWindow(orders);
                if(window.ShowDialog() == true) {

                }
            }
        }

        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var orders = LViewOrders.ItemsSource as List<Order>;
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Excel.Worksheet sheet = workbook.Worksheets[1];


            sheet.Cells[1, 1] = "Дата и время";
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[2, 1]].Merge();
            sheet.Cells[1, 2] = "Услуга";
            sheet.Range[sheet.Cells[1, 2], sheet.Cells[2, 2]].Merge();
            sheet.Cells[1, 3] = "Стоимость услуги";
            sheet.Range[sheet.Cells[1, 3], sheet.Cells[2, 3]].Merge();
            Excel.Range eqHeader = sheet.Range[sheet.Cells[1, 4], sheet.Cells[1, 6]];
            eqHeader.Merge();
            eqHeader.Value = "Оборудование";
            sheet.Cells[2, 4] = "Название";
            sheet.Cells[2, 5] = "Цена";
            sheet.Cells[2, 6] = "Количество";
            sheet.Cells[1, 7] = "Сумма";
            sheet.Range[sheet.Cells[1, 7], sheet.Cells[2, 7]].Merge();
            sheet.Cells[1, 8] = "ИТОГО";
            sheet.Range[sheet.Cells[1, 8], sheet.Cells[2, 8]].Merge();

            Excel.Range mainHeader = sheet.Range[sheet.Cells[1, 1], sheet.Cells[2, 8]];
            mainHeader.Interior.Color = Excel.XlRgbColor.rgbDeepSkyBlue;
            mainHeader.Font.Color = Excel.XlRgbColor.rgbWhite;
            mainHeader.Font.Bold = true;
            mainHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            var rowIndex = 3;
            foreach (var order in orders)
            {
                sheet.Cells[rowIndex, 1] = order.OrderDateTime.ToString("dd.MM.yyyy HH:mm");
                sheet.Cells[rowIndex, 2] = order.Service.Name;
                sheet.Cells[rowIndex, 3] = order.Service.Price;
                var startIndex = rowIndex;
                if (order.OrderEquipment.ToList().Count > 0)
                {
                    foreach (var item in order.OrderEquipment.ToList())
                    {
                        sheet.Cells[rowIndex, 4] = item.Equipment.Name;
                        sheet.Cells[rowIndex, 5] = item.Equipment.Price;
                        sheet.Cells[rowIndex, 6] = item.Amount;
                        sheet.Cells[rowIndex, 7] = $"=F{rowIndex}*E{rowIndex}";
                        rowIndex++;
                    }

                }
                else
                {
                    Excel.Range eqRange = sheet.Range[sheet.Cells[rowIndex, 4], sheet.Cells[rowIndex, 6]];
                    eqRange.Merge();
                    eqRange.Value = "Нет оборудования";
                    sheet.Cells[rowIndex, 7] = " - ";
                    rowIndex++;
                }
                sheet.Cells[rowIndex - 1, 8] = $"=SUM(G{startIndex}:G{rowIndex - 1}) + C{startIndex}";
                sheet.Range[sheet.Cells[startIndex, 1], sheet.Cells[rowIndex - 1, 1]].Merge();
                sheet.Range[sheet.Cells[startIndex, 2], sheet.Cells[rowIndex - 1, 2]].Merge();
                sheet.Range[sheet.Cells[startIndex, 3], sheet.Cells[rowIndex - 1, 3]].Merge();
                sheet.Range[sheet.Cells[startIndex, 8], sheet.Cells[rowIndex - 1, 8]].Merge();
            }
            Excel.Range sumHeader = sheet.Range[sheet.Cells[rowIndex, 1], sheet.Cells[rowIndex, 7]];
                sumHeader.Merge();
            sumHeader.Font.Bold = true;
            sumHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            sumHeader.Value = "ИТОГО ПО ВСЕМ ЗАЯВКАМ:";
            sheet.Cells[rowIndex, 8] = $"=SUM(H3:H{rowIndex - 1})";
            sheet.Columns.AutoFit();
            sheet.UsedRange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
            sheet.UsedRange.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle =
            sheet.UsedRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
            sheet.UsedRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle =
            sheet.UsedRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
            sheet.UsedRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
            Excel.XlLineStyle.xlContinuous;
            sheet.Columns.AutoFit();
            app.Visible = true;
        }
    }
}
