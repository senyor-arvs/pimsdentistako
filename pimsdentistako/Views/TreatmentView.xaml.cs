using pimsdentistako.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for TreatmentView.xaml
    /// </summary>
    public partial class TreatmentView : UserControl
    {
        private static readonly OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" +
              @"Data Source=|DataDirectory|\Database\PIMSDentistaKo.mdb;" +
              "Jet OLEDB:Database Password = dQXpe}3]?Rx&.7zh*cZ^;");
        ObservableCollection<Treatment> treatmentList = new ObservableCollection<Treatment>();
        public TreatmentView()
        {
            InitializeComponent();
            treatmentDataGrid.ItemsSource = treatmentList;
            loadData();
        }

        private void addTreatmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadData()
        {
            try
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("SELECT * FROM Treatment", conn);
                OleDbDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Treatment treatment = new Treatment(
                        treatmentID: dataReader["treatmentID"].ToString(),
                        treatmentName: dataReader["treatmentName"].ToString()
                    );
                    treatmentList.Add(treatment);
                }
            } 
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
    }
}
