using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.Linq;
using System.Data.OleDb;
using System.Windows.Controls;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;

namespace pimsdentistako.DBHelpers
{
    public class PatientHelper
    {
        private static int initSearch = 0;
        private static TextBlock textCount;
        private static ComboBox comboBox;

        private static readonly string myTable = DBNames.TableNames.PATIENT;
        private static readonly string[] col = DBNames.ColumnNames.Patient;
        private static int currentSelection;


        public static ObservableCollection<Patient> PatientList { get; set; }
        public static DataGrid MyDataGrid { get; set; }
        public static ComboBox MyComboBox { get => comboBox; set
            {
                comboBox = value;
                comboBox.ItemsSource = SearchByMenu.MENU;
                comboBox.SelectedIndex = 0;
            }
        }

        public static class SearchByMenu
        {
            public static readonly string PATIENT_NO = "Patient No.";
            public static readonly string FIRST_NAME = "First Name";
            public static readonly string LAST_NAME = "Last Name";
            public static readonly string NULL = "NUN";
            private static ObservableCollection<string> mENU = new ObservableCollection<string>()
            {
                 PATIENT_NO, FIRST_NAME, LAST_NAME
            };
            public static ObservableCollection<string> MENU { get => mENU; }

            public static string GetActiveColumn(string selected_searchby_menu)
            {
                string[] col = DBNames.ColumnNames.Patient;
                if (DatabaseHelper.compare(selected_searchby_menu, PATIENT_NO)) return col[0];
                else if (DatabaseHelper.compare(selected_searchby_menu, FIRST_NAME)) return col[1];
                else if (DatabaseHelper.compare(selected_searchby_menu, LAST_NAME)) return col[3];
                return NULL;
            }

            public static string GetActiveMenu(string selected_searchby_menu)
            {
                string[] col = DBNames.ColumnNames.Patient;
                if (DatabaseHelper.compare(selected_searchby_menu, PATIENT_NO)) return PATIENT_NO;
                else if (DatabaseHelper.compare(selected_searchby_menu, FIRST_NAME)) return FIRST_NAME;
                else if (DatabaseHelper.compare(selected_searchby_menu, LAST_NAME)) return LAST_NAME;
                return NULL;
            }
        }

        //attach here the textblock where the count of patient are displayed
        public static TextBlock TextCount { get => textCount; set => textCount = value; }

        //WORKING
        public static bool InitList() //Initialize the List of patients retreive from database
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                PatientList = new ObservableCollection<Patient>();
                OleDbCommand getAllPatientsCommand = new OleDbCommand("SELECT * FROM " + myTable, GetConnectionObject());
                OleDbDataReader dataReader = getAllPatientsCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    Patient patient = new Patient
                    {
                        PatientID = dataReader[col[0]].ToString(),
                        PatientFirstName = dataReader[col[1]].ToString(),
                        PatientMiddleName = dataReader[col[2]].ToString(),
                        PatientLastName = dataReader[col[3]].ToString(),
                        PatientSuffix = dataReader[col[4]].ToString(),
                        PatientNickname = dataReader[col[5]].ToString(),
                        PatientCivilStatus = dataReader[col[6]].ToString(),
                        PatientAddress = dataReader[col[7]].ToString(),
                        PatientEmail = dataReader[col[8]].ToString(),
                        PatientMobileNumber = dataReader[col[9]].ToString(),
                        PatientHomeNumber = dataReader[col[10]].ToString(),
                        PatientBirthdate = dataReader[col[11]].ToString(),
                        PatientSex = dataReader[col[12]].ToString(),
                        PatientReferredBy = dataReader[col[13]].ToString(),
                        PatientOccupation = dataReader[col[14]].ToString(),
                        PatientCompany = dataReader[col[15]].ToString(),
                        PatientOfficeNumber = dataReader[col[16]].ToString(),
                        PatientFaxNumber = dataReader[col[17]].ToString()
                    };

                    PatientList.Add(patient);
                }
                reorderPatientList();
                actionState = true;
            }
            catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static bool PatientSearch(string active_column, string pattern)
        {
            bool foundSomething = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                StringBuilder query = new StringBuilder();
                query.Append("SELECT * FROM ").Append(myTable)
                    .Append(" WHERE ").Append(active_column)
                    .Append(" LIKE ").Append("'%").Append(pattern).Append("%'");

                OleDbCommand searchCommand = new OleDbCommand(query.ToString(), GetConnectionObject());
                OleDbDataReader dataReader = searchCommand.ExecuteReader();

                PatientList.Clear();
                PatientList = new ObservableCollection<Patient>();

                while (dataReader.Read())
                {
                    Patient patient = new Patient
                    {
                        PatientID = dataReader[col[0]].ToString(),
                        PatientFirstName = dataReader[col[1]].ToString(),
                        PatientMiddleName = dataReader[col[2]].ToString(),
                        PatientLastName = dataReader[col[3]].ToString(),
                        PatientSuffix = dataReader[col[4]].ToString(),
                        PatientNickname = dataReader[col[5]].ToString(),
                        PatientCivilStatus = dataReader[col[6]].ToString(),
                        PatientAddress = dataReader[col[7]].ToString(),
                        PatientEmail = dataReader[col[8]].ToString(),
                        PatientMobileNumber = dataReader[col[9]].ToString(),
                        PatientHomeNumber = dataReader[col[10]].ToString(),
                        PatientBirthdate = dataReader[col[11]].ToString(),
                        PatientSex = dataReader[col[12]].ToString(),
                        PatientReferredBy = dataReader[col[13]].ToString(),
                        PatientOccupation = dataReader[col[14]].ToString(),
                        PatientCompany = dataReader[col[15]].ToString(),
                        PatientOfficeNumber = dataReader[col[16]].ToString(),
                        PatientFaxNumber = dataReader[col[17]].ToString()
                    };

                    PatientList.Add(patient);
                }
                reorderPatientList();
                foundSomething = PatientList.Any();
            } catch(Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }

            return foundSomething;
        }


        #region DATAGRID LISTENERS AND HANDLERS
        //implement this trategy in all helpers
        //listens to data grid selections - you must add data grid to helper before everything
        public static void ListenToDataGrid()
        {
            currentSelection = MyDataGrid.SelectedIndex;
        }

        //RETURNS the currently selected Dentist based on the inside currentSelection attribute
        public static Patient CurrentlySelectedPatient()
        {
            return DatabaseHelper.IsSelectedIndexValid(currentSelection, PatientList.Count) ? PatientList[currentSelection] : null;
        }

        //check if handler is grabbing a null dentist
        public static bool IsCurrentlySelectedNull()
        {
            return CurrentlySelectedPatient() == null;
        }

        //Use this for displaying data current selected on DataGrid on the Provided TextBoxes
        public static void DisplaySelected(params TextBox[] textBox)
        {
            if (DatabaseHelper.IsSelectedIndexValid(currentSelection, PatientList.Count))
            {
                Patient selected = PatientList[currentSelection];
                string[] values = {
                    selected.PatientFirstName,
                    selected.PatientMiddleName,
                    selected.PatientLastName,
                    selected.PatientSuffix,
                    selected.PatientNickname,
                    selected.PatientSex,
                    selected.PatientCivilStatus,
                    selected.PatientAddress,
                    selected.PatientEmail,
                    selected.PatientMobileNumber,
                    selected.PatientHomeNumber,
                    selected.PatientBirthdate,
                    selected.PatientReferredBy,
                    selected.PatientOccupation,
                    selected.PatientCompany,
                    selected.PatientOfficeNumber,
                    selected.PatientFaxNumber,
                    Patient.GetAgeByBirth(selected.PatientBirthdate)
                };

                for (int i = 0; i < textBox.Length; i++)
                {
                    textBox[i].Text = values[i];
                }
            }
        }
        #endregion DATAGRID LISTENERS AND HANDLERS

        #region SEARCH LISTENERS
        public static void ListenToSearch(TextBox searchTextBox)
        {
            if (!DatabaseHelper.CheckNullEmptyInput(searchTextBox).Equals(DatabaseHelper.BLANK_INPUT) && DatabaseHelper.IsSelectedIndexValid(MyComboBox.SelectedIndex, SearchByMenu.MENU.Count))
            {
                string activeCol = PatientHelper.SearchByMenu.GetActiveColumn(PatientHelper.SearchByMenu.MENU[MyComboBox.SelectedIndex]);
                string pattern = searchTextBox.Text;
                bool action = PatientHelper.PatientSearch(activeCol, pattern);
                initSearch++;
            }
        }

        public static void ListenToClearSearch(TextBox searchTextBox)
        {
            if(initSearch > 0)
            {
                initSearch = 0;
                MyComboBox.SelectedIndex = 0;
                searchTextBox.Clear();
                PatientHelper.InitList();
                PatientHelper.reorderPatientList();
            }
        }

        public static void ListenToComboBoxSelection(TextBlock textBlock)
        {
            if (DatabaseHelper.IsSelectedIndexValid(MyComboBox.SelectedIndex, SearchByMenu.MENU.Count))
            {
                textBlock.Text = SearchByMenu.GetActiveMenu(PatientHelper.SearchByMenu.MENU[MyComboBox.SelectedIndex]);
            }
        }

        #endregion SEARCH LISTENERS

        public static void reorderPatientList() //sort the list ascending use only i needed to sort list
        {
            if (TextCount != null) TextCount.Text = PatientList.Count.ToString();
            PatientList = new ObservableCollection<Patient>(PatientList.OrderBy(i => Convert.ToUInt64(i.PatientID))); //sort via ID
            if(MyDataGrid != null) MyDataGrid.ItemsSource = PatientList;
        }
    }
}
