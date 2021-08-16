using System.Windows;
using System.Windows.Controls;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for DentalRecordsView.xaml
    /// </summary>
    public partial class DentalRecordsWindow : Window
    {
        private string patientName;
        private string patientNumber;
        public DentalRecordsWindow(string PatientName, string PatientNumber)
        {
            InitializeComponent();
            this.patientName = PatientName;
            this.patientNumber = PatientNumber;

            patientNameTxt.Text = patientName;
            patientNumberTxt.Text = patientNumber;
        }

        private void datagridDentalRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
