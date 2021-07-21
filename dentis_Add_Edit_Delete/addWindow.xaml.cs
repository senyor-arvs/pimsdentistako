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
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;

using dentis_Add_Edit_Delete.DBElements;
using dentis_Add_Edit_Delete.DBHelpers;

namespace dentis_Add_Edit_Delete
{
    /// <summary>
    /// Interaction logic for addWindow.xaml
    /// </summary>
    public partial class addWindow : Window
    {
    
        public addWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.DEBUG = true;
            //Create a Dentist
            Dentist dentist = new Dentist //dentist ID no input
            {
                DentistFirstName = DatabaseHelper.CheckNullEmptyInput(txtAddFirst),
                DentistMiddleName = DatabaseHelper.CheckNullEmptyInput(txtAddMiddle),
                DentistLastName = DatabaseHelper.CheckNullEmptyInput(txtAddLast),
                DentistSuffix = DatabaseHelper.CheckNullEmptyInput(txtAddSuffix),
                DentistLicenseNumber = DatabaseHelper.CheckNullEmptyInput(txtAddLicense),
                DentistPTRNumber = DatabaseHelper.CheckNullEmptyInput(txtAddPtr)
            };

            //ADD DENTIST
            bool actionResult = DentistHelper.AddDentist(dentist);
            
            if(actionResult)
            {
                MessageBox.Show("Dentist was Added Successfully.");
            } else
            {
                MessageBox.Show("An error occured while adding the Dentist.");
            }
            this.Close();
        }


        
    }
}
