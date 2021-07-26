using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;
using static pimsdentistako.DBHelpers.EmergencyInfoHelper;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for AddPatientView.xaml
    /// </summary>
    public partial class AddPatientView : Window
    {
        public AddPatientView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //make form draggable
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void savePatientButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.DEBUG = true;
            if(IsConditionOnePassed() && IsConditionTwoPassed())
            {
                bool result = PatientHelper.AddPatient(GeneratePatient(
                firstNameTxtBox, middleNameTxtBox, lastNameTxtBox,
                suffixTxtBox, nickNameTxtBox, civilStatusTxtBox,
                addressTxtBox, emailTxtBox, mobileNumberTxtBox,
                homeNumberTxtBox, birthDateTxtBox, sexTxtBox,
                referredTxtBox, occupationTxtBox, companyTxtBox,
                officeNumberTxtBox, faxTxtBox
                ), GenerateEmergencyInfo(
                    emergencyNameTxtBox, emergencyHomeNumberTxtBox,
                    emergencyRelationshipTxtBox, emergencyOfficeNumTxtBox
                ));

                if (result)
                {
                    DatabaseHelper.DisplayDialog("Adding Patient", "The Patient was Added Successfully.");
                    this.Close();
                } else
                {
                    DatabaseHelper.DisplayErrorDialog("Adding Patient", "Something went wrong while adding patient.");
                }
            } else
            {
                DatabaseHelper.DisplayErrorDialog("Adding Patient", "Some fields are required to be filed.\nEither First Name, Middle Name, Last Name and Mobile Number.\nFor emergency Information, Name and Mobile Number is required.");
            }
        }

        private bool IsConditionOnePassed()
        {
            return (!DatabaseHelper.IsTextBoxTextNullEmpty(firstNameTxtBox) ||
                !DatabaseHelper.IsTextBoxTextNullEmpty(middleNameTxtBox) ||
                 !DatabaseHelper.IsTextBoxTextNullEmpty(lastNameTxtBox)) &&
                 !DatabaseHelper.IsTextBoxTextNullEmpty(mobileNumberTxtBox);
        }

        private bool IsConditionTwoPassed()
        {
            return (!DatabaseHelper.IsTextBoxTextNullEmpty(emergencyNameTxtBox)
                && !DatabaseHelper.IsTextBoxTextNullEmpty(emergencyHomeNumberTxtBox)) ||
                !DatabaseHelper.IsTextBoxTextNullEmpty(emergencyRelationshipTxtBox)
                || !DatabaseHelper.IsTextBoxTextNullEmpty(emergencyOfficeNumTxtBox);
        }

        private Patient GeneratePatient(params object[] items)
        {
            List<string> values = new List<string>();
            foreach(object item in items)
            {
                if(item is TextBox)
                {
                    values.Add(DatabaseHelper.CheckNullEmptyInput((TextBox)item));
                } else
                {
                    values.Add(DatabaseHelper.CheckNullEmptyInput((DatePicker)item));
                }
            }
            Patient patient = new Patient
            {
                PatientFirstName = values[0],
                PatientMiddleName = values[1],
                PatientLastName = values[2],
                PatientSuffix = values[3],
                PatientNickname = values[4],
                PatientCivilStatus = values[5],
                PatientAddress = values[6],
                PatientEmail = values[7],
                PatientMobileNumber = values[8],
                PatientHomeNumber = values[9],
                PatientBirthdate = values[10],
                PatientSex = values[11],
                PatientReferredBy = values[12],
                PatientOccupation = values[13],
                PatientCompany = values[14],
                PatientOfficeNumber = values[15],
                PatientFaxNumber = values[16]
            };
            return patient;
        }

        private EmergencyInfo GenerateEmergencyInfo(params TextBox[] txbx)
        {
            List<string> values = new List<string>();
            foreach (TextBox item in txbx)
            {
                if (item is TextBox)
                {
                    values.Add(DatabaseHelper.CheckNullEmptyInput(item));
                }
            }
            EmergencyInfo emergencyInfo = new EmergencyInfo
            {
                ContactName = values[0],
                ContactNumber = values[1],
                ContactRelationship = values[2],
                ContactOfficeNumber = values[3],
                ContactOfficeNumberRemarks = OfficeNumberRemarks.UNIQUE_TO_PATIENT
                
            };
            return emergencyInfo;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void birthDateTxtBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ageTxtBox.Text = Patient.GetAgeByBirth(birthDateTxtBox.Text);
        }
    }
}
