using System;
using System.Collections.ObjectModel;
using System.Text;

using System.Linq;
using System.Data.OleDb;
using System.Windows.Controls;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using System.Collections.Generic;

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

        #region SEARCH BY MENU CLASS
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

            [Obsolete("Redundant Functionality", true)]
            public static string GetActiveMenu(string selected_searchby_menu)
            {
                string[] col = DBNames.ColumnNames.Patient;
                if (DatabaseHelper.compare(selected_searchby_menu, PATIENT_NO)) return PATIENT_NO;
                else if (DatabaseHelper.compare(selected_searchby_menu, FIRST_NAME)) return FIRST_NAME;
                else if (DatabaseHelper.compare(selected_searchby_menu, LAST_NAME)) return LAST_NAME;
                return NULL;
            }
        }
        #endregion SEARCH BY MENU CLASS

        #region FIELD MENUS
        public static class FieldMenu
        {
            //menu for sex
            public static readonly string MALE = "Male";
            public static readonly string FEMALE = "Female";
            //menu for civil status
            public static readonly string SINGLE = "Single";
            public static readonly string MARRIED = "Married";
            public static readonly string DIVORCED = "Divorced";
            public static readonly string SEPARATED = "Separated";

            //company
            public static readonly string DEFAULT_COMPANY = "Regular Patient";

            //menu names
            public static readonly int SEX_MENU = 0;
            public static readonly int CIVIL_STAT_MENU = 1;

            private static ObservableCollection<string> sexMenu = new ObservableCollection<string>()
            {
                 FEMALE, MALE
            };

            private static ObservableCollection<string> civilStatMenu = new ObservableCollection<string>()
            {
                 SINGLE, MARRIED, DIVORCED, SEPARATED
            };
            public static ObservableCollection<string> SexMenu { get => sexMenu;}
            public static ObservableCollection<string> CivilStatMenu { get => civilStatMenu;}

            public static int GetIndexOfCivilStatusItem(string patientCivilStatus)
            {
                return CivilStatMenu.IndexOf(patientCivilStatus);
            }

            public static int GetIndexOfSexItem(string patientSex)
            {
                return SexMenu.IndexOf(patientSex);
            }
        }

        #endregion FIELD MENUS

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
                OleDbCommand getAllPatientsCommand = DatabaseHelper.SelectAllCommand(myTable);
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

        //RETURNS the last record
        public static Patient GetLastRecord()
        {
            Patient patient = new Patient();
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                
                OleDbCommand getLastRecordCommand = DatabaseHelper.LastRecordCommand(myTable, col[0]);
                OleDbDataReader dataReader = getLastRecordCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    patient = new Patient
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
                }
            }
            catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return patient;
        }

        //WORKING
        public static bool AddPatient(Patient patient, EmergencyInfo emergency)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand insertCommand = DatabaseHelper.AddCommand(myTable, col, 1, 17, new List<string>
                {
                    patient.PatientFirstName,
                    patient.PatientMiddleName,
                    patient.PatientLastName,
                    patient.PatientSuffix,
                    patient.PatientNickname,
                    patient.PatientCivilStatus,
                    patient.PatientAddress,
                    patient.PatientEmail,
                    patient.PatientMobileNumber,
                    patient.PatientHomeNumber,
                    patient.PatientBirthdate,
                    patient.PatientSex,
                    patient.PatientReferredBy,
                    patient.PatientOccupation,
                    patient.PatientCompany,
                    patient.PatientOfficeNumber,
                    patient.PatientFaxNumber,
                });

                bool result = insertCommand.ExecuteNonQuery() > 0;
                bool addEmergencyInfo = false;
                bool added = false;
                Patient patientRecent  = new Patient();

                if (result)
                {
                    patientRecent = GetLastRecord();
                    addEmergencyInfo = EmergencyInfoHelper.AddEmergencyInfo(patientRecent.PatientID, emergency);
                }
                if (result && addEmergencyInfo)
                {
                    PatientList.Add(patientRecent);
                    added = true;
                    reorderPatientList();
                }
                actionState = result && addEmergencyInfo && added;
            } catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static bool UpdatePatient(Patient patient, EmergencyInfo emergencyInfo)
        {
            bool actionState = false;

            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("UPDATE ").Append(myTable).Append(" SET ")
                    .Append(col[1]).Append("= ?, ")
                    .Append(col[2]).Append("= ?, ")
                    .Append(col[3]).Append("= ?, ")
                    .Append(col[4]).Append("= ?, ")
                    .Append(col[5]).Append("= ?, ")
                    .Append(col[6]).Append("= ?, ")
                    .Append(col[7]).Append("= ?, ")
                    .Append(col[8]).Append("= ?, ")
                    .Append(col[9]).Append("= ?, ")
                    .Append(col[10]).Append("= ?, ")
                    .Append(col[11]).Append("= ?, ")
                    .Append(col[12]).Append("= ?, ")
                    .Append(col[13]).Append("= ?, ")
                    .Append(col[14]).Append("= ?, ")
                    .Append(col[15]).Append("= ?, ")
                    .Append(col[16]).Append("= ?, ")
                    .Append(col[17]).Append("= ? ")
                    .Append("WHERE ").Append(col[0]).Append("= ?");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@fname", patient.PatientFirstName));
                updateCommand.Parameters.Add(new OleDbParameter("@midName", patient.PatientMiddleName));
                updateCommand.Parameters.Add(new OleDbParameter("@lastName", patient.PatientLastName));
                updateCommand.Parameters.Add(new OleDbParameter("@suffix", patient.PatientSuffix));
                updateCommand.Parameters.Add(new OleDbParameter("@nickName", patient.PatientNickname));
                updateCommand.Parameters.Add(new OleDbParameter("@civilStat", patient.PatientCivilStatus));
                updateCommand.Parameters.Add(new OleDbParameter("@addr", patient.PatientAddress));
                updateCommand.Parameters.Add(new OleDbParameter("@email", patient.PatientEmail));
                updateCommand.Parameters.Add(new OleDbParameter("@mobileNum", patient.PatientMobileNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@homeNum", patient.PatientHomeNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@birth", patient.PatientBirthdate));
                updateCommand.Parameters.Add(new OleDbParameter("@sex", patient.PatientSex));
                updateCommand.Parameters.Add(new OleDbParameter("@referr", patient.PatientReferredBy));
                updateCommand.Parameters.Add(new OleDbParameter("@occupation", patient.PatientOccupation));
                updateCommand.Parameters.Add(new OleDbParameter("@comp", patient.PatientCompany));
                updateCommand.Parameters.Add(new OleDbParameter("@officeNum", patient.PatientOfficeNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@fax", patient.PatientFaxNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@id", patient.PatientID));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                bool updateEmergencyInfo = false;

                if (affectedRows) updateEmergencyInfo = EmergencyInfoHelper.UpdateEmergencyInformation(emergencyInfo);
                bool removed = false;

                if (affectedRows && updateEmergencyInfo)
                {
                    Patient toRemove = PatientList.Single(i => i.PatientID.Equals(patient.PatientID)); //access the old one
                    removed = PatientList.Remove(toRemove); //remove it
                    PatientList.Add(patient); //add the new one
                    reorderPatientList();
                }
                actionState = affectedRows && removed;
            } catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }

            return actionState;
        }


        #region DATAGRID LISTENERS AND HANDLERS
        //implement this trategy in all helpers
        //listens to data grid selections - you must add data grid to helper before everything
        public static void ListenToDataGrid()
        {
            currentSelection = MyDataGrid.SelectedIndex;
        }

        //RETURNS the currently selected Patient based on the inside currentSelection attribute
        public static Patient CurrentlySelectedPatient()
        {
            return DatabaseHelper.IsSelectedIndexValid(currentSelection, PatientList.Count) ? PatientList[currentSelection] : null;
        }

        //check if handler is grabbing a null patient
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
                string activeCol = SearchByMenu.GetActiveColumn(SearchByMenu.MENU[MyComboBox.SelectedIndex]);
                string pattern = searchTextBox.Text;
                _ = PatientSearch(activeCol, pattern);
                if(initSearch == 0) initSearch++;
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
                //textBlock.Text = SearchByMenu.GetActiveMenu(PatientHelper.SearchByMenu.MENU[MyComboBox.SelectedIndex]);
                textBlock.Text = PatientHelper.SearchByMenu.MENU[MyComboBox.SelectedIndex];
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
