using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace pimsdentistako
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly OleDbConnection _CONNECTION_OBJ = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" +
                    @"Data Source=|DataDirectory|\Database\PIMSDentistaKo.mdb;" +
                    "Jet OLEDB:Database Password = dQXpe}3]?Rx&.7zh*cZ^;");
    }
}
