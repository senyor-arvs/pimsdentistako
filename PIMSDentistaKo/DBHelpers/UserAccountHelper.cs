using System;
using System.Text;
using System.Linq;
using System.Data.OleDb;
using System.Collections.ObjectModel;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using System.Windows.Controls;

namespace pimsdentistako.DBHelpers
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

        internal static bool UpdatePassword(TextBox oldPasswordTxtBox, TextBox newPasswordTxtBox, TextBox retypePasswordTxtBox)
        {
            if (!DatabaseHelper.IsTextBoxTextNullEmpty(oldPasswordTxtBox))
            {
                if (UserAccountHelper.IsOldPasswordMatch(oldPasswordTxtBox.Text.Trim()))
                {
                    if (newPasswordTxtBox.Text.Trim().Length >= 4 || retypePasswordTxtBox.Text.Trim().Length >= 4)
                    {
                        if (!DatabaseHelper.IsTextBoxTextNullEmpty(newPasswordTxtBox) && !DatabaseHelper.IsTextBoxTextNullEmpty(retypePasswordTxtBox) && DatabaseHelper.compare(newPasswordTxtBox.Text.Trim(), retypePasswordTxtBox.Text.Trim()))
                        {
                            if (!DatabaseHelper.compare(newPasswordTxtBox.Text, UserAccountHelper.GetAdmin().Password))
                            {
                                bool result = UserAccountHelper.UpdateUserAccount(newPasswordTxtBox.Text.Trim());
                                if (result)
                                {
                                    return result;
                                }
                                else
                                {
                                    DatabaseHelper.DisplayErrorDialog("Error", "Something went wrong while changing your password.");
                                }
                            }
                            else
                            {
                                DatabaseHelper.DisplayWarningDialog("Warning", "Old password is the same as your new password.");
                            }
                        }
                        else
                        {
                            DatabaseHelper.DisplayErrorDialog("New Password", "New password did not match. Please retype it again.");
                        }
                    }
                    else
                    {
                        DatabaseHelper.DisplayErrorDialog("Invalid Password", "Password length must be of length greater or equal to 4");
                    }
                }
                else
                {
                    DatabaseHelper.DisplayErrorDialog("Old Password", "We cannot verify if its you. Please check your old password.");
                }
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog("Changing Password", "Please enter the required fields.");
            }
            return false;
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
                query.Append("UPDATE ").Append(myTable).Append(" SET ").Append("[")
                    .Append(col[3]).Append("]").Append("= ? ")
                    .Append("WHERE ").Append(col[1]).Append("= ?");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@pass", new_password));
                updateCommand.Parameters.Add(new OleDbParameter("@id", GetAdmin().DentistID.ToString()));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                bool removed = false;
                UserAccount toRemove = AccountList.Single(i => i.UserAccountRemarks.Equals(AccoutRemarks.ADMIN)); //access the old one
                UserAccount newUser = new UserAccount
                {
                    DentistID = toRemove.DentistID,
                    Username = toRemove.Username,
                    Password = new_password,
                    UserAccountRemarks = AccoutRemarks.ADMIN
                };
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

        public static string GetAdminFullNameByQuery()
        {
            string fUllName = "Administrator";
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                StringBuilder query = new StringBuilder();
                query.Append("SELECT * FROM ").Append(myTable)
                    .Append(" WHERE ").Append(col[4]).Append(" = ").Append("'")
                    .Append(AccoutRemarks.ADMIN).Append("'");

                OleDbCommand getAdminCommand = new OleDbCommand(query.ToString(), GetConnectionObject());
                OleDbDataReader dataReader = getAdminCommand.ExecuteReader();

                fUllName = dataReader[col[2]].ToString();
            } catch(Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return fUllName;
        }

        public static string GetAdminFullName()
        {
            return GetAdmin().Username;
        }

        public static UserAccount GetAdmin()
        {
            return AccountList.Single(i => i.UserAccountRemarks.Equals(AccoutRemarks.ADMIN));
        }

        private static void reorderAccountList()
        {
            //TODO ATTACH HERE THE DATA GRID OF USER ACCOUNT
            AccountList = new ObservableCollection<UserAccount>(AccountList.OrderBy(i => Convert.ToUInt64(i.DentistID))); //sort via ID
        }


        internal static bool IsOldPasswordMatch(string input_password)
        {
            return DatabaseHelper.compare(GetAdmin().Password, input_password);
        }
    }
}
