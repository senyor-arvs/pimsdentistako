using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using dentis_Add_Edit_Delete.DBElements;
using dentis_Add_Edit_Delete.DBHelpers;

namespace dentis_Add_Edit_Delete
{
    /// <summary>
    /// Interaction logic for editWindow.xaml
    /// </summary>
    public partial class editWindow : Window
    {
        private Dentist active;
        public editWindow()
        {
            InitializeComponent();
            active = DentistHelper.CurrentlySelectedDentist();
            DentistHelper.DisplaySelected(txtEditId, txtEditFirst, txtEditMiddle, txtEditLast, txtEditSuffix, txtEditLicense, txtEditPtr);
        }

        private void btnEditCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEditConfirm_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.DEBUG = true;
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
                MessageBox.Show("Dentist was Successfuly Updated.");
            }
            else
            {
                MessageBox.Show("Ann error occured while updating Dentist.\nNothing was changed.");
            }
            this.Close();
        }
    }
    
}
