using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.OleDb;
using System.Collections.ObjectModel;
using dentis_Add_Edit_Delete.DBElements;
using static dentis_Add_Edit_Delete.DBHelpers.DatabaseHelper;

namespace dentis_Add_Edit_Delete.DBHelpers
{
    public class UserAccountHelper
    {
        //non admin acc are linked to dentist helper
        //admin acc have specific function get admin
        private static string[] col = DBNames.ColumnNames.User_Account;
        private static string myTable = DBNames.TableNames.USER_ACC;

        public static ObservableCollection<UserAccount> AccountList { get; set; }

        public class AccoutRemarks
        {
            public static readonly string NON_ADMIN = "dentist";
            public static readonly string ADMIN = "admin";
            public static readonly string SUPER_ADMIN = "super_admin";
        }

        //WORKING
        //INIT UNDER USER ACCOUNTS
        public static bool InitList()
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                AccountList = new ObservableCollection<UserAccount>();
                OleDbCommand getAllAccountCommand = new OleDbCommand("SELECT * FROM " + DBNames.TableNames.USER_ACC, GetConnectionObject());
                OleDbDataReader dataReader = getAllAccountCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    UserAccount user = new UserAccount
                    {
                        DentistID = dataReader[col[1]].ToString(),
                        Username = dataReader[col[2]].ToString(),
                        Password = dataReader[col[3]].ToString(),
                        UserAccountRemarks = dataReader[col[4]].ToString(),
                    };
                    if (user.UserAccountRemarks.Equals(AccoutRemarks.SUPER_ADMIN)) continue; //dont read super admin accounts
                    AccountList.Add(user);
                }
                reorderAccountList();
                actionState = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        //WORKING
        //set initList param to true if you want to init list automatically - on user acc section it can be false
        public static bool AddUserAccount(UserAccount user) //automatically called inside dentisthelper
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO ").Append(myTable).Append(" (")
                    .Append(col[1]).Append(", ")
                    .Append(col[2]).Append(", ")
                    .Append(Preserved(new String(col[3]))).Append(", ")
                    .Append(col[4]).Append(") ")
                    .Append(" VALUES (@id, @uname, @pass, @remark)");

                OleDbCommand insertCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                insertCommand.Parameters.Add(new OleDbParameter("@id", user.DentistID));
                insertCommand.Parameters.Add(new OleDbParameter("@uname", user.Username));
                insertCommand.Parameters.Add(new OleDbParameter("@pass", user.Password));
                insertCommand.Parameters.Add(new OleDbParameter("@remark", user.UserAccountRemarks));

                actionState = insertCommand.ExecuteNonQuery() > 0;
                if (actionState)
                {
                    AccountList.Add(user); //if adding is successfull it will add the result to list
                    reorderAccountList();
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

        //TODO UNTESTED UPDATE USER ACC
        public static bool UpdateUserAccount(string new_password) //use only for admin - only password can be changed
        {
            bool actionState = false;

            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("UPDATE ").Append(myTable).Append(" SET ")
                    .Append(col[3]).Append("='@pass' ")
                    .Append("WHERE ").Append(col[3]).Append("=@remark");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@pass", new_password));
                updateCommand.Parameters.Add(new OleDbParameter("@remark", AccoutRemarks.ADMIN));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                bool removed = false;
                UserAccount toRemove = AccountList.Single(i => i.UserAccountRemarks.Equals(AccoutRemarks.ADMIN)); //access the old one
                UserAccount newUser = toRemove;
                newUser.Password = new_password;
                if (affectedRows)
                {
                    removed = AccountList.Remove(toRemove); //remove it
                }
                if (affectedRows && removed)
                {
                    AccountList.Add(newUser);
                    reorderAccountList();
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

        //WORKING CALLED INSIDE UPDATE DENTIST
        public static bool UpdateUserName(string newUserName, string dentistID) //use only for admin - only password can be changed
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("UPDATE ").Append(myTable).Append(" SET ")
                    .Append(col[2]).Append("= ? ")
                    .Append("WHERE ").Append(col[1]).Append("= ?");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@uname", newUserName));
                updateCommand.Parameters.Add(new OleDbParameter("@id", dentistID));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                bool removed = false;

                UserAccount toRemove = AccountList.Single(i => i.DentistID.Equals(dentistID)); //access the old one
                UserAccount newUser = toRemove;
                newUser.Username = newUserName;
                if (affectedRows)
                {
                    removed = AccountList.Remove(toRemove); //remove it
                }
                if (affectedRows && removed)
                {
                    AccountList.Add(newUser);
                    reorderAccountList();
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
        public static bool DeleteUserAccount(string dentistID) //use only for non admin accounts - Automatically Called Inside DentistHelper
        {
            bool actionState = false;
            if (DentistHelper.IsAdmin(dentistID)) return actionState;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM " + myTable + " WHERE " + col[1] + "='" + dentistID + "';", GetConnectionObject());
                bool actionResult = deleteCommand.ExecuteNonQuery() > 0;

                bool removed = false;
                if (actionResult)
                {
                    UserAccount toRemove = AccountList.Single(i => i.DentistID.Equals(dentistID));
                    removed = AccountList.Remove(toRemove);
                }
                actionState = actionResult && removed;
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

        private static void reorderAccountList()
        {
            //TODO ATTACH HERE THE DATA GRID OF USER ACCOUNT
            AccountList = new ObservableCollection<UserAccount>(AccountList.OrderBy(i => Convert.ToUInt64(i.DentistID))); //sort via ID
        }
    }
}
