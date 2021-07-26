using System.Windows;

using pimsdentistako.DBHelpers;
using pimsdentistako.DBElements;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for DentistEditWindow.xaml
    /// </summary>
    public partial class DentistEditWindow : Window
    {
        private Dentist active;

        public DentistEditWindow()
        {
            InitializeComponent();
            active = DentistHelper.CurrentlySelectedDentist();
            DentistHelper.DisplaySelected(txtEditFirst, txtEditMiddle, txtEditLast, txtEditSuffix, txtEditLicense, txtEditPtr);
        }



        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            Dentist dentist = new Dentist
            {
                DentistID = active.DentistID,
                DentistFirstName = DatabaseHelper.CheckNullEmptyInput(txtEditFirst),
                DentistMiddleName = DatabaseHelper.CheckNullEmptyInput(txtEditMiddle),
                DentistLastName = DatabaseHelper.CheckNullEmptyInput(txtEditLast),
                DentistSuffix = DatabaseHelper.CheckNullEmptyInput(txtEditSuffix),
                DentistLicenseNumber = DatabaseHelper.CheckNullEmptyInput(txtEditLicense),
                DentistPTRNumber = DatabaseHelper.CheckNullEmptyInput(txtEditPtr)
            };

            bool actionResult = DentistHelper.UpdateDentist(dentist);
            if (actionResult)
            {
                DatabaseHelper.DisplayDialog("Update Dentist", "Dentist was Successfuly Updated.");
            }
            else
            {
               DatabaseHelper.DisplayErrorDialog("Update Dentist", "Ann error occured while updating Dentist.\nNothing was changed.");
            }
            this.Close();
        }
    }
}
