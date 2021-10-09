using System.Windows;
using System.Windows.Controls;
using pimsdentistako.Callbacks;

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

        public IWindowCloseListener WindowCloseListener { get; set; }

        private void datagridDentalRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) this.DragMove();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
