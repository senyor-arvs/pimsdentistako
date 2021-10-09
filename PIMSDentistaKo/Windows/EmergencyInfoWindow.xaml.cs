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
using System.Windows.Shapes;
using pimsdentistako.Callbacks;
using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for EmergencyInfoWindow.xaml
    /// </summary>
    public partial class EmergencyInfoWindow : Window
    {
        public IWindowCloseListener WindowCloseListener { get; set; }

        public Patient ActivePatient { get; set; }

        public EmergencyInfoWindow()
        {
            InitializeComponent();
        }

        private void displayEmergencyInfo()
        {
            txtBlockPatientName.Text = ActivePatient.PatientFullName;
            txtBlockPatientNumber.Text = ActivePatient.PatientID;

            EmergencyInfo emergencyInfo = EmergencyInfoHelper.RetrievePatientEmergencyInfo(ActivePatient.PatientID);
            EmergencyInfoHelper.DisplayInfoOnWindow(ActivePatient, emergencyInfo,
                emergencyInfoNameWin,
                emergencyInfoRelationshipWin,
                emergencyInfoHomeWin,
                emergencyInfoOfficeWin);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            displayEmergencyInfo();
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void btnCloseEmergencyWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }
    }
}
