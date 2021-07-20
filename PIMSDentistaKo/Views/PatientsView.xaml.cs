using System;
using System.Collections.Generic;
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
using pimsdentistako.Views;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : UserControl
    {
        public PatientsView()
        {
            InitializeComponent();
        }

        private void patientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Master_File_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dentalRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            DentalRecordsView dentalRecordsView = new DentalRecordsView();
            dentalRecordsView.Show();
        }
    }
}
