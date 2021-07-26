using System.Windows;
using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for DentistAddWindow.xaml
    /// </summary>
    public partial class DentistAddWindow : Window
    {
        public DentistAddWindow()
        {
            InitializeComponent();
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            //Create a Dentist
            if((!DatabaseHelper.IsTextBoxTextNullEmpty(txtAddFirst) || !DatabaseHelper.IsTextBoxTextNullEmpty(txtAddLast)) && !DatabaseHelper.IsTextBoxTextNullEmpty(txtAddLicense))
            {
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

                if (actionResult)
                {
                    DatabaseHelper.DisplayDialog("Adding Dentist", "Dentist was Added Successfully.");
                }
                else
                {
                    DatabaseHelper.DisplayErrorDialog("Adding Dentist", "An error occured while adding the Dentist.");
                }
                this.Close();
            } 
            else
            {
                DatabaseHelper.DisplayErrorDialog("Adding Dentist", "Some fields are required to be filled.\nEither First Name, Last Name and License Number");
            }
        }
         
    }
}
