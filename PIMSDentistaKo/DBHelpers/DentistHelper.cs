using System;
using System.Text;
using System.Data.OleDb;
using System.Linq;
using System.Collections.ObjectModel;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using static pimsdentistako.DBHelpers.UserAccountHelper;
using System.Windows.Controls;
using System.Collections.Generic;

namespace pimsdentistako.DBHelpers
{
    public class DentistHelper
    {
        private static TextBlock textCount;
        private static readonly string myTable = DBNames.TableNames.DENTIST;
        private static readonly string[] col = DBNames.ColumnNames.Dentist;
        private static int currentSelection;

        //attach here the textblock where the count of patient are displayed
        public static TextBlock TextCount { get => textCount; set => textCount = value; }

        public static ObservableCollection<Dentist> DentistList { get; set; }
        public static DataGrid MyDataGrid { get; set; }

        //WORKING
        public static bool InitList() //RUN ON DENTIST VIEW BEFORE GETTING THE DENTIST OBJECT - This will also refresh the List
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                DentistList = new ObservableCollection<Dentist>();
                OleDbCommand getAllDentistCommand = DatabaseHelper.SelectAllCommand(myTable);
                OleDbDataReader dataReader = getAllDentistCommand.ExecuteReader();;

                while (dataReader.Read())
                {
                    Dentist dentist = new Dentist
                    {
                        DentistID = dataReader[col[0]].ToString(),
                        DentistFirstName = dataReader[col[1]].ToString(),
                        DentistMiddleName = dataReader[col[2]].ToString(),
                        DentistLastName = dataReader[col[3]].ToString(),
                        DentistSuffix = dataReader[col[4]].ToString(),
                        DentistLicenseNumber = dataReader[col[5]].ToString(),
                        DentistPTRNumber = dataReader[col[6]].ToString(),

                    };

                    DentistList.Add(dentist);
                }
                reorderDentistList();

                UserAccountHelper.InitList(); //you also need to initialize this list since they are linked together Dont Forget It

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

        //RETURNS the last record
        public static Dentist GetLastRecord()
        {
            Dentist dentist = new Dentist();
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand getLastRecordCommand = DatabaseHelper.LastRecordCommand(myTable, col[0]);
                OleDbDataReader dataReader = getLastRecordCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    dentist = new Dentist
                    {
                        DentistID = dataReader[col[0]].ToString(),
                        DentistFirstName = dataReader[col[1]].ToString(),
                        DentistMiddleName = dataReader[col[2]].ToString(),
                        DentistLastName = dataReader[col[3]].ToString(),
                        DentistSuffix = dataReader[col[4]].ToString(),
                        DentistLicenseNumber = dataReader[col[5]].ToString(),
                        DentistPTRNumber = dataReader[col[6]].ToString(),
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
            return dentist;
        }

        //UNDONE WORKING - POSSIBLE ROLLBACK MUST BE IMPLEMENTED
        //ADDING DENTIST - Returns Boolean Representing the state of action if false the action fails
        public static bool AddDentist(Dentist dentist)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand insertCommand = DatabaseHelper.AddCommand(myTable, col, 1, 6, new List<string>
                {
                    dentist.DentistFirstName,
                    dentist.DentistMiddleName,
                    dentist.DentistLastName,
                    dentist.DentistSuffix,
                    dentist.DentistLicenseNumber,
                    dentist.DentistPTRNumber
                });

                bool initial_adding = insertCommand.ExecuteNonQuery() > 0;

                //RETREIVE IT AGAIN SINCE IT IS SURELY THE LAST RECORD
                if (initial_adding)
                {
                    dentist = GetLastRecord(); //DO NOT HANDLE ERROR 
                }

                UserAccount user = new UserAccount();
                user.DentistID = dentist.DentistID;
                user.Username = dentist.DentistFullName; //user name is dentist full name

                if (GetMaxID(DBNames.TableNames.USER_ACC) == 2) //2 MEANS SUPER ADMIN ONLY EXIST
                {
                    //MAKE ACCOUNT ADMIN - custom password can only be applied after account creation
                    user.Password = AccoutRemarks.ADMIN;
                    user.UserAccountRemarks = AccoutRemarks.ADMIN;
                }
                else
                {
                    //MAKE ACCOUNT NON ADMIN
                    user.Password = AccoutRemarks.NON_ADMIN;
                    user.UserAccountRemarks = AccoutRemarks.NON_ADMIN;
                }

                bool second_adding = false;
                if (initial_adding) second_adding = UserAccountHelper.AddUserAccount(user); //create an account also for the dentist
                actionState = initial_adding && second_adding;
                if (actionState)
                {
                    DentistList.Add(dentist); //if adding is successfull it will add the result to list
                    reorderDentistList();
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
            return actionState;
        }

        //WORKING
        public static bool UpdateDentist(Dentist dentist)
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
                    .Append(col[6]).Append("= ? ")
                    .Append("WHERE ").Append(col[0]).Append("= ?");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@fname", dentist.DentistFirstName));
                updateCommand.Parameters.Add(new OleDbParameter("@midName", dentist.DentistMiddleName));
                updateCommand.Parameters.Add(new OleDbParameter("@lastName", dentist.DentistLastName));
                updateCommand.Parameters.Add(new OleDbParameter("@suffix", dentist.DentistSuffix));
                updateCommand.Parameters.Add(new OleDbParameter("@lic", dentist.DentistLicenseNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@ptr", dentist.DentistPTRNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@id", dentist.DentistID));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                bool updateAcc = true;
                if (affectedRows) updateAcc = UpdateUserName(dentist.DentistFullName, dentist.DentistID);
                bool removed = false;

                if (affectedRows && updateAcc)
                {
                    Dentist toRemove = DentistList.Single(i => i.DentistID.Equals(dentist.DentistID)); //access the old one
                    removed = DentistList.Remove(toRemove); //remove it
                    DentistList.Add(dentist); //add the new one
                    reorderDentistList();
                }
                actionState = affectedRows && removed;
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

        //WORKING
        public static bool DeleteDentist(string dentistID)
        {
            bool actionState = false;
            if (DentistHelper.IsAdmin(dentistID)) return actionState; //YOU CAN'T DELETE ADMIN ACCOUNT
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand deleteCommand = DatabaseHelper.DeleteCommand(myTable, col[0], dentistID, true);
                bool affectedRows = deleteCommand.ExecuteNonQuery() > 0;

                bool accountRemoved = false;
                if (affectedRows) accountRemoved = UserAccountHelper.DeleteUserAccount(dentistID); //delete user account also
                bool removed = false;
                if (affectedRows && accountRemoved)
                {
                    Dentist toRemove = DentistList.Single(i => i.DentistID.Equals(dentistID)); //get the item
                    removed = DentistList.Remove(toRemove);
                    reorderDentistList(); //reorder list
                }
                actionState = affectedRows && accountRemoved && removed;
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
        public static void reorderDentistList() //sort the list ascending use only i needed to sort list
        {
            if (TextCount != null) TextCount.Text = DentistList.Count.ToString();
            DentistList = new ObservableCollection<Dentist>(DentistList.OrderBy(i => Convert.ToUInt64(i.DentistID))); //sort via ID
            if(MyDataGrid != null) MyDataGrid.ItemsSource = DentistList;
        }

        #region DATAGRID LISTENERS AND HANDLERS
        //implement this trategy in all helpers
        //listens to data grid selections - you must add data grid to helper before everything
        public static void ListenToDataGrid()
        {
            currentSelection = MyDataGrid.SelectedIndex;
        }

        //RETURNS the currently selected Dentist based on the inside currentSelection attribute
        public static Dentist CurrentlySelectedDentist()
        {
            return DatabaseHelper.IsSelectedIndexValid(currentSelection, DentistList.Count) ? DentistList[currentSelection] : null;
        }

        //check if handler is grabbing a null dentist
        public static bool IsCurrentlySelectedNull()
        {
            return CurrentlySelectedDentist() == null;
        }

        //Use this for displaying data current selected on DataGrid on the Provided TextBoxes
        public static void DisplaySelected(params TextBox[] textBox)
        {
            if (DatabaseHelper.IsSelectedIndexValid(currentSelection, DentistList.Count))
            {
                Dentist selected = DentistList[currentSelection];
                string[] values = {
                selected.DentistFirstName,
                selected.DentistMiddleName,
                selected.DentistLastName,
                selected.DentistSuffix,
                selected.DentistLicenseNumber,
                selected.DentistPTRNumber};

                for (int i = 0; i < textBox.Length; i++)
                {
                    textBox[i].Text = values[i];
                }
            }
        }
        #endregion DATAGRID LISTENERS AND HANDLERS

        //CHECKS IF PROVIDED ID IS ADMINISTRATOR
        public static bool IsAdmin(string dentistID)
        {
            bool isAdmin = false;
            //do not allow deletion of admin account
            if (UserAccountHelper.AccountList.Single(i => i.DentistID.Equals(dentistID)).UserAccountRemarks.Equals(AccoutRemarks.ADMIN))
            {
                isAdmin = true;
            }
            return isAdmin;
        }

    }
}
