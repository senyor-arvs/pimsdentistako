using System.Windows;
using System.Windows.Controls;

using pimsdentistako.DBHelpers;

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
            PatientHelper.ListenToDataGrid();
            PatientHelper.DisplaySelected(nameTxtBox,MiddleName,LastName,Suffix,Nickname,Sex,CivilStatus,Address,Email,MobileNumber,HomeNumber,DateOfBirth,RefferedBy,Occupation,Company,OfficeNumber,FaxNumber,Age);
        }
        private void Master_File_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dentalRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            DentalRecordsView dentalRecordsView = new DentalRecordsView();
            dentalRecordsView.Show();
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
            AddPatientView addPatientView = new AddPatientView();
            addPatientView.Show();
        }
    }
}
