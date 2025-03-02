using SpeedLinkApplication.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SpeedLinkApplication
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    { 
        public static Entities.SpeedLinkDatabaseEntities Context = new Entities.SpeedLinkDatabaseEntities();
        public static User AuthUser;
    }
}
