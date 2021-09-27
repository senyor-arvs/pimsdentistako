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

using pimsdentistako.DBHelpers;
using pimsdentistako.DBElements;
using pimsdentistako.Callbacks;
using System.Diagnostics;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : UserControl, IWindowCloseListener
    {
        public PatientsView()
        {
            InitializeComponent();
            DatabaseHelper.Init();
            PatientHelper.MyDataGrid = patientDataGrid;
            PatientHelper.TextCount = txtTotalPatient; //attach the count textblock to auto update whenever the list changes
            PatientHelper.MyComboBox = searchByComboBox;
            PatientHelper.InitList();
        }

        private void patientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            treatmentPlanButton.IsEnabled = true;
            emergencyInfoButton.IsEnabled = true;
            dentalRecordButton.IsEnabled = true;
            PatientHelper.ListenToDataGrid();
            PatientHelper.DisplaySelected(nameTxtBox,MiddleName,LastName,Suffix,Nickname,Sex,CivilStatus,Address,Email,MobileNumber,HomeNumber,DateOfBirth,RefferedBy,Occupation,Company,OfficeNumber,FaxNumber,Age);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            PatientHelper.ListenToSearch(txtBoxSearch);
        }

        private void clearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            PatientHelper.ListenToClearSearch(txtBoxSearch);
        }

        private void searchByComboBox_SelChanged(object sender, SelectionChangedEventArgs e)
        {
            PatientHelper.ListenToComboBoxSelection(searchTxt);
        }

        private void AddPatientButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditPatientWindow addPatientView = new AddEditPatientWindow(0);
            addPatientView.WindowCloseListener = this;
            addPatientView.Show();
        }

        private void editPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PatientHelper.IsCurrentlySelectedNull())
            {
                AddEditPatientWindow addPatientView = new AddEditPatientWindow(1);
                addPatientView.WindowCloseListener = this;
                addPatientView.Show();
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_PATIENT_SELECTION);
            }
        }

        private void deletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (!PatientHelper.IsCurrentlySelectedNull())
            {
                DeleteConfimationWindow deleteConfimationWindow = new DeleteConfimationWindow(
                DeleteConfimationWindow.DeleteWindowModes.PATIENT_MODE);
                deleteConfimationWindow.WindowCloseListener = this;
                deleteConfimationWindow.Show();
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_PATIENT_SELECTION);
            }
        }

        private void dentalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PatientHelper.IsCurrentlySelectedNull())
            {
                var patientNumber = PatientHelper.CurrentlySelectedPatient().PatientID;
                var patientName = PatientHelper.CurrentlySelectedPatient().PatientFullName;
                DentalRecordsWindow dentalRecordsWindow = new DentalRecordsWindow(PatientName: patientName, PatientNumber: patientNumber);
                dentalRecordsWindow.WindowCloseListener = this;
                dentalRecordsWindow.Show();
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_PATIENT_SELECTION);
            }
        }

        private void treatmentPlanButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PatientHelper.IsCurrentlySelectedNull())
            {
                var patientNumber = PatientHelper.CurrentlySelectedPatient().PatientID;
                var patientName = PatientHelper.CurrentlySelectedPatient().PatientFullName;
                TreatmentPlanWindow treatmentPlanWindow = new TreatmentPlanWindow(patientName, patientNumber);
                treatmentPlanWindow.WindowCloseListener = this;
                treatmentPlanWindow.Show();
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_PATIENT_SELECTION);
            }
        }

        private void emergencyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if(!PatientHelper.IsCurrentlySelectedNull())
            {
                EmergencyInfoWindow emergencyInfoWindow = new EmergencyInfoWindow();
                emergencyInfoWindow.ActivePatient = PatientHelper.CurrentlySelectedPatient();
                emergencyInfoWindow.WindowCloseListener = this;
                emergencyInfoWindow.Show();
            } 
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_PATIENT_SELECTION);
            }
        }

        public void OnWindowClose(bool isClosed)
        {
           Window.GetWindow(this).IsEnabled = isClosed;
        }
    }
}
