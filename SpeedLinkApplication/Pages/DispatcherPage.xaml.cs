﻿using System;
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

namespace SpeedLinkApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для DispatcherPage.xaml
    /// </summary>
    public partial class DispatcherPage : Page
    {
        public DispatcherPage()
        {
            InitializeComponent();
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DispatcherOrdersPage());
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage());
        }

        private void BtnAddress_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddressPage());
        }
    }
}
