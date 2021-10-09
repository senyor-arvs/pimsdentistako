using System.Windows.Controls;
using System.Windows;
using pimsdentistako.ViewModels;
using pimsdentistako.Windows;
using pimsdentistako.Callbacks; 

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for AppointmentsView.xaml
    /// </summary>
    public partial class AppointmentsView : UserControl, IWindowCloseListener
    {
        public AppointmentsView()
        {
            InitializeComponent();
        }

        private void appointmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DataContext = new PatientSelectionViewModel();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAppointmentWindow updateAppointmentWindow = new UpdateAppointmentWindow();
            updateAppointmentWindow.WindowCloseListener = this;
            updateAppointmentWindow.Show();
        }

        public void OnWindowClose(bool isClosed)
        {
            Window.GetWindow(this).IsEnabled = isClosed;
        }
    }
}
