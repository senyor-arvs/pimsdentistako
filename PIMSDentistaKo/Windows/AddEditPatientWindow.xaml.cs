using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;
using static pimsdentistako.DBHelpers.EmergencyInfoHelper;
using pimsdentistako.Callbacks;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for AddPatientView.xaml
    /// </summary>
    public partial class AddEditPatientWindow : Window
    {
        private readonly string AddingWindowTitle = "Add Patient Information";
        private readonly string EditWindowTitle = "Edit Patient Information";
        private readonly string AddingPatient = "Adding Patient";
        private readonly string EditingPatient = "Editing Patient";

        private int mode;

        public IWindowCloseListener WindowCloseListener { get; set; }

        public AddEditPatientWindow(int mode)
        {
            InitializeComponent();
            this.mode = mode;
            sexComboBox.ItemsSource = PatientHelper.FieldMenu.SexMenu;
            sexComboBox.SelectedIndex = 0;

            civilStatusComboBox.ItemsSource = PatientHelper.FieldMenu.CivilStatMenu;
            civilStatusComboBox.SelectedIndex = 0;

            MainWindowAddEdit.Title = this.mode == 0 ? AddingWindowTitle : EditWindowTitle;
            AddEditTitle.Text = MainWindowAddEdit.Title;

            if (this.mode != 0) InitEditMode();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //make form draggable
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }
        private void InitEditMode()
        {
            Patient selected = PatientHelper.CurrentlySelectedPatient();
            firstNameTxtBox.Text = selected.PatientFirstName;
            middleNameTxtBox.Text = selected.PatientMiddleName;
            lastNameTxtBox.Text = selected.PatientLastName;
            suffixTxtBox.Text = selected.PatientSuffix;
            nickNameTxtBox.Text = selected.PatientNickname;
            civilStatusComboBox.SelectedIndex = PatientHelper.FieldMenu.GetIndexOfCivilStatusItem(selected.PatientCivilStatus);
            addressTxtBox.Text = selected.PatientAddress;
            emailTxtBox.Text = selected.PatientEmail;
            mobileNumberTxtBox.Text = selected.PatientMobileNumber;
            homeNumberTxtBox.Text = selected.PatientHomeNumber;
            birthDateTxtBox.Text = selected.PatientBirthdate;
            sexComboBox.SelectedIndex = PatientHelper.FieldMenu.GetIndexOfSexItem(selected.PatientSex);
            referredTxtBox.Text = selected.PatientReferredBy;
            occupationTxtBox.Text = selected.PatientOccupation;
            companyTxtBox.Text = selected.PatientCompany;
            officeNumberTxtBox.Text = selected.PatientOfficeNumber;
            faxTxtBox.Text = selected.PatientFaxNumber;

            EmergencyInfo emergencyInfo = RetrievePatientEmergencyInfo(selected.PatientID);
            DisplayInfoOnWindow(selected, emergencyInfo, 
                emergencyNameTxtBox, 
                emergencyRelationshipTxtBox,
                emergencyHomeNumberTxtBox,
                emergencyOfficeNumTxtBox);
        }

        private void savePatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsConditionOnePassed() && IsConditionTwoPassed())
            {
                Patient generatedPatient = GeneratePatient(
                firstNameTxtBox, middleNameTxtBox, lastNameTxtBox,
                suffixTxtBox, nickNameTxtBox, civilStatusComboBox,
                addressTxtBox, emailTxtBox, mobileNumberTxtBox,
                homeNumberTxtBox, birthDateTxtBox, sexComboBox,
                referredTxtBox, occupationTxtBox, companyTxtBox,
                officeNumberTxtBox, faxTxtBox
                );

                EmergencyInfo generatedEmergencyInfo = GenerateEmergencyInfo(
                    emergencyNameTxtBox, emergencyHomeNumberTxtBox,
                    emergencyRelationshipTxtBox, emergencyOfficeNumTxtBox
                );

                bool result;

                if (mode == 0) //ADD MODE
                {
                    result = PatientHelper.AddPatient(generatedPatient, generatedEmergencyInfo);
                }
                else //EDIT MODE
                {
                    generatedPatient.PatientID = PatientHelper.CurrentlySelectedPatient().PatientID;
                    generatedEmergencyInfo.PatientID = generatedPatient.PatientID;
                    result = PatientHelper.UpdatePatient(generatedPatient, GenerateEmergencyInfoUpdate(
                        generatedPatient.PatientID,
                        emergencyNameTxtBox, emergencyHomeNumberTxtBox,
                        emergencyRelationshipTxtBox, emergencyOfficeNumTxtBox));
                }
                if (result)
                {
                    if (mode == 0)
                    {
                        DatabaseHelper.DisplayDialog(AddingPatient, "The Patient was Added Successfully.");
                    }
                    else
                    {
                        DatabaseHelper.DisplayDialog(EditingPatient, "The Patient was Updated Successfully.");
                    }
                    this.Close();
                }
                else
                {
                    if (mode == 0)
                    {
                        DatabaseHelper.DisplayErrorDialog(AddingPatient, "Something went wrong while adding patient.");
                    }
                    else
                    {
                        DatabaseHelper.DisplayErrorDialog(EditingPatient, "Something went wrong while updating patient.");
                    }
                }
            }
            else
            {
                DatabaseHelper.DisplayErrorDialog(mode == 0 ? AddingPatient : EditingPatient, "Some fields are required to be filed.\nEither First Name, Middle Name, Last Name and Mobile Number.\nFor emergency Information, Name and Mobile Number is required.");
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
                } else if(item is DatePicker)
                {
                    values.Add(DatabaseHelper.CheckNullEmptyInput((DatePicker)item));
                } else if(item is ComboBox)
                {
                    values.Add(DatabaseHelper.BLANK_INPUT); //dummy added value
                }
            }
            Patient patient = new Patient
            {
                PatientFirstName = values[0],
                PatientMiddleName = values[1],
                PatientLastName = values[2],
                PatientSuffix = values[3],
                PatientNickname = values[4],
                PatientCivilStatus = DatabaseHelper.IsSelectedIndexValid(civilStatusComboBox.SelectedIndex, PatientHelper.FieldMenu.CivilStatMenu.Count) ? 
                PatientHelper.FieldMenu.CivilStatMenu[civilStatusComboBox.SelectedIndex] : DatabaseHelper.BLANK_INPUT,
                PatientAddress = values[6],
                PatientEmail = values[7],
                PatientMobileNumber = values[8],
                PatientHomeNumber = values[9],
                PatientBirthdate = values[10],
                PatientSex = DatabaseHelper.IsSelectedIndexValid(sexComboBox.SelectedIndex, PatientHelper.FieldMenu.SexMenu.Count) ? 
                PatientHelper.FieldMenu.SexMenu[sexComboBox.SelectedIndex] : DatabaseHelper.BLANK_INPUT,
                PatientReferredBy = values[12],
                PatientOccupation = values[13],
                PatientCompany = values[14].Equals(DatabaseHelper.BLANK_INPUT) ? PatientHelper.FieldMenu.DEFAULT_COMPANY : values[14],
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
                ContactOfficeNumberRemarks = OfficeNumberRemarks.SAME_TO_PATIENT

            };
            return emergencyInfo;
        }

        private EmergencyInfo GenerateEmergencyInfoUpdate(string patientID, params TextBox[] txbx)
        {
            EmergencyInfo old = EmergencyInfoHelper.RetrievePatientEmergencyInfo(patientID);

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
                PatientID = DatabaseHelper.IsValueNull(old.PatientID) ? patientID : old.PatientID,
                ContactName = values[0],
                ContactNumber = values[1],
                ContactRelationship = values[2],
                ContactOfficeNumber = values[3],
            };

            if (!DatabaseHelper.IsValueNull(old.ContactOfficeNumberRemarks))
            {
                emergencyInfo.ContactOfficeNumberRemarks = old.ContactOfficeNumberRemarks;
            } else
            {
                emergencyInfo.ContactOfficeNumberRemarks = EmergencyInfoHelper.OfficeNumberRemarks.SAME_TO_PATIENT;
            }
            return emergencyInfo;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void birthDateTxtBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ageTxtBox.Text = Patient.GetAgeByBirth(birthDateTxtBox.Text);
        }

        private void MainWindowAddEdit_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void MainWindowAddEdit_Closed(object sender, System.EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

    }
}
