using mshtml;
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
    /// Логика взаимодействия для OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        public OrderDetailsWindow(Order order)
        {
            InitializeComponent();
            BtnPrint.Visibility = App.AuthUser.Role.Id == 2 & order.OrderStatus.Id <= 2 ? Visibility.Collapsed:
                Visibility.Visible;
            string html = "<!DOCTYPE html>\r\n" +
                "<html lang=\"ru\">" +
                "\r\n<head>\r\n    " +
                "<meta charset=\"UTF-8\">\r\n    " +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                "\r\n<title>Справка по заявке</title>\r\n" +
                "<style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f9f9f9;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 20px;\r\n        }\r\n        .container {\r\n            max-width: 800px;\r\n            margin: 0 auto;\r\n            background-color: #fff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);\r\n        }\r\n        h1 {\r\n            text-align: center;\r\n            color: #6200ea;\r\n            margin-bottom: 20px;\r\n        }\r\n        h2 {\r\n            color: #333;\r\n            margin-top: 20px;\r\n            margin-bottom: 10px;\r\n        }\r\n        .section {\r\n            margin-bottom: 20px;\r\n        }\r\n        .section p {\r\n            margin: 5px 0;\r\n        }\r\n        table {\r\n            width: 100%;\r\n            border-collapse: collapse;\r\n            margin-bottom: 20px;\r\n        }\r\n        table th, table td {\r\n            padding: 10px;\r\n            text-align: left;\r\n            border-bottom: 1px solid #ddd;\r\n        }\r\n        table th {\r\n            background-color: #6200ea;\r\n            color: #fff;\r\n        }\r\n        table tr:nth-child(even) {\r\n            background-color: #f9f9f9;\r\n        }\r\n        .comment {\r\n            margin-top: 20px;\r\n        }\r\n        .comment .comment-text {\r\n            padding: 10px;\r\n            border: 1px solid #ddd;\r\n            border-radius: 4px;\r\n            background-color: #f9f9f9;\r\n            font-family: Arial, sans-serif;\r\n            font-size: 14px;\r\n            white-space: pre-wrap; /* Сохраняет форматирование текста */\r\n        }\r\n    </style>" +
                "</head>\r\n<body>\r\n    " +
                "<div class=\"container\">\r\n" +
                "<h1>Справка по заявке</h1>\r\n\r\n" +
                "<!-- Информация о клиенте -->\r\n" +
                "<div class=\"section\">\r\n" +
                "<h2>Информация о клиенте</h2>\r\n" +
                $"<p><strong>Клиент:</strong> {order.Client.FullName}</p>\r\n" +
                $"<p><strong>Номер телефона:</strong> {order.Client.PhoneNumber}</p>\r\n" +
                $"<p><strong>Email:</strong> {order.Client.Email}</p>\r\n" +
                $"<p><strong>Адрес:</strong> {order.Address.FullName}</p>\r\n        " +
                $"</div>\r\n\r\n        " +
                $"<!-- Информация об услуге -->\r\n       " +
                $"<div class=\"section\">\r\n           " +
                $" <h2>Информация об услуге</h2>\r\n            " +
                $"<p><strong>Услуга:</strong> {order.Service.Name}</p>\r\n            " +
                $"<p><strong>Стоимость услуги:</strong> {order.Service.Price} руб.</p>\r\n        " +
                $"</div>\r\n\r\n        " +
                $"<!-- Перечень затраченного оборудования -->\r\n        " +
                $"<div class=\"section\">\r\n            " +
                $"<h2>Установленное оборудование</h2>\r\n            ";

            if (order.OrderEquipment.ToList().Count > 0)
            {
                html += $"<table>\r\n                " +
                $"<thead>\r\n                    " +
                $"<tr>\r\n                        " +
                $"<th>Название</th>\r\n                        " +
                $"<th>Цена (руб.)</th>\r\n                        " +
                $"<th>Количество</th>\r\n                        " +
                $"<th>Стоимость (руб.)</th>\r\n                   " +
                $" </tr>\r\n                " +
                $"</thead>\r\n               " +
                $" <tbody>\r\n                    ";

                foreach (var item in order.OrderEquipment.ToList())
                {
                    html += $"<tr>\r\n                        " +
                        $"<td>{item.Equipment.Name}</td>\r\n                        " +
                        $"<td>{item.Equipment.Price} руб.</td>\r\n                       " +
                        $"<td>{item.Amount}</td>\r\n                        " +
                        $"<td>{item.Amount * item.Equipment.Price} руб.</td>\r\n                    " +
                        $"</tr>\r\n                   ";

                }
                html += $"   </tbody>\r\n          " +
                        $"      <tfoot>\r\n                " +
                        $"    <tr>\r\n                      " +
                        $"  <td colspan=\"3\" style=\"text-align: right;\">" +
                        $"<strong>Итого:</strong></td>\r\n                        " +
                        $"<td><strong>{order.OrderEquipment.ToList().Sum(x => x.Amount * x.Equipment.Price)} руб.</strong></td>\r\n" +
                        $"</tr>\r\n                " +
                        $"</tfoot>\r\n            " +
                        $"</table>\r\n       ";
            }
            else
            {

                html += $"<div class=\"comment-text\">" +
                                     $"Оборудование не было установлено." +
                                     $"</div>\r\n";
            }


            html += $" </div>\r\n\r\n        " +
            $"<!-- Дополнительный комментарий -->\r\n        " +
            $" <div class=\"section comment\">\r\n            " +
            $"<h2>Описание</h2>\r\n            " +
            $"<div class=\"comment-text\">" +
            (!string.IsNullOrWhiteSpace(order.Description) ? order.Description : "Описание отсутствует.") +
            $"</div>\r\n        </div>" +
            $"" +
            $" <div class=\"section\">\r\n            " +
            $"<h2>Прочие данные</h2>\r\n            " +
            $"<p><strong>Статус заявки:</strong> {order.OrderText}</p>\r\n" +
            $"<p><strong>Заявка создана:</strong> {order.CreationDateTime:dd.MM.yyyy HH:mm:ss}</p>\r\n" +
            $"<p><strong>Заявку принял(а):</strong> {order.User.FullName}</p>\r\n";
            if(order.User1 != null && order.OrderStatus.Id == 3)
                html += $"<p><strong>Заявку выполнил(а):</strong> {order.User1.FullName}</p>\r\n";
            html += "</div>" + 
            $"" +
            $"</div>" +
            $"\r\n</body>\r\n</html>";
            WBrowserDetails.NavigateToString(html);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var doc = WBrowserDetails.Document as IHTMLDocument2;
            doc.execCommand("Print");
        }
    }
}
