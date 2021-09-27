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
using pimsdentistako.Windows;
using pimsdentistako.Callbacks;
using pimsdentistako.ViewModels;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for PatientSelectionView.xaml
    /// </summary>
    public partial class PatientSelectionView : UserControl, IWindowCloseListener
    {
        public PatientSelectionView()
        {
            InitializeComponent();
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            AddEditTreatmentPlanWindow addEditTreatmentPlanWindow = new AddEditTreatmentPlanWindow();
            addEditTreatmentPlanWindow.WindowCloseListener = this;
            addEditTreatmentPlanWindow.Show();
        }

        public void OnWindowClose(bool isClosed)
        {
            Window.GetWindow(this).IsEnabled = isClosed;
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DataContext = new AppointmentsViewModel();
        }
    }
}
