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
            addPatientView.Show();
        }

        private void editPatientButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditPatientWindow addPatientView = new AddEditPatientWindow(1);
            addPatientView.Show();
        }

        private void deletePatient_Click(object sender, RoutedEventArgs e)
        {
            //TODO IMPLEMENT CONFIRMATION DIALOG ON PATIENT DELETE
            MessageBox.Show("Deleted.");
        }

        private void dentalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            var patientNumber = PatientHelper.CurrentlySelectedPatient().PatientID;
            var patientName = PatientHelper.CurrentlySelectedPatient().PatientFullName;
            DentalRecordsWindow dentalRecordsWindow = new DentalRecordsWindow(PatientName: patientName, PatientNumber: patientNumber);
            dentalRecordsWindow.Show();
        }
    }
}
