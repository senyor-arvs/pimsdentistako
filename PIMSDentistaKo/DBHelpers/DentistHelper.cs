using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data.OleDb;
using System.Linq;
using System.Collections.ObjectModel;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using static pimsdentistako.DBHelpers.UserAccountHelper;

namespace pimsdentistako.DBHelpers
{
    //TODO LINK TO USER ACCOUNT FUNCTIONS
    public class DentistHelper
    {
        private static readonly string myTable = DBNames.TableNames.DENTIST;
        private static readonly string[] col = DBNames.ColumnNames.Dentist;

        public static ObservableCollection<Dentist> DentistList { get; set; }

        public static bool InitList() //RUN ON DENTIST VIEW BEFORE GETTING THE DENTIST OBJECT - This will also refresh the List
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                DentistList = new ObservableCollection<Dentist>();
                OleDbCommand getAllDentistCommand = new OleDbCommand("SELECT * FROM " + DBNames.TableNames.DENTIST, GetConnectionObject());
                OleDbDataReader dataReader = getAllDentistCommand.ExecuteReader();

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

        //ADDING DENTIST - Returns Boolean Representing the state of action if false the action fails
        public static bool AddDentist(Dentist dentist)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                UserAccount user = new UserAccount();
                user.DentistID = dentist.DentistID;
                user.Username = dentist.getFullName(); //user name is dentist full name

                if (GetAvailableID(DBNames.TableNames.USER_ACC) == 2) //2 MEANS SUPER ADMIN ONLY EXIST
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

                StringBuilder query = new StringBuilder();
                query.Append("@INSERT INTO ").Append(myTable).Append(" (")
                    .Append(col[1]).Append(", ")
                    .Append(col[2]).Append(", ")
                    .Append(col[3]).Append(", ")
                    .Append(col[4]).Append(", ")
                    .Append(col[5]).Append(", ")
                    .Append(col[6]).Append(")")
                    .Append(" VALUES (@fName, @midName, @lastName, @suffix, @lic, @ptr)");

                OleDbCommand insertCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                insertCommand.Parameters.Add(new OleDbParameter("@fName", dentist.DentistFirstName));
                insertCommand.Parameters.Add(new OleDbParameter("@midName", dentist.DentistMiddleName));
                insertCommand.Parameters.Add(new OleDbParameter("@lastName", dentist.DentistLastName));
                insertCommand.Parameters.Add(new OleDbParameter("@suffix", dentist.DentistSuffix));
                insertCommand.Parameters.Add(new OleDbParameter("@lic", dentist.DentistLicenseNumber));
                insertCommand.Parameters.Add(new OleDbParameter("@ptr", dentist.DentistPTRNumber));

                bool initial_adding = insertCommand.ExecuteNonQuery() > 0;
                bool second_adding = false;
                if(initial_adding) second_adding = UserAccountHelper.AddUserAccount(user, true); //create an account also for the dentist
                actionState = initial_adding && second_adding;
                if (actionState)
                {
                    DentistList.Add(dentist); //if adding is successfull it will add the result to list
                    reorderDentistList();
                }
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

        public static bool UpdateDentist(Dentist dentist)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("@UPDATE ").Append(myTable).Append(" SET ")
                    .Append(col[1]).Append("='@fname', ")
                    .Append(col[2]).Append("='@midName', ")
                    .Append(col[3]).Append("='@lastName', ")
                    .Append(col[4]).Append("='@suffix', ")
                    .Append(col[5]).Append("='@lic', ")
                    .Append(col[6]).Append("='@ptr' ")
                    .Append("WHERE ").Append(col[0]).Append("=@id");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@id", dentist.DentistID));
                updateCommand.Parameters.Add(new OleDbParameter("@fName", dentist.DentistFirstName));
                updateCommand.Parameters.Add(new OleDbParameter("@midName", dentist.DentistMiddleName));
                updateCommand.Parameters.Add(new OleDbParameter("@lastName", dentist.DentistLastName));
                updateCommand.Parameters.Add(new OleDbParameter("@suffix", dentist.DentistSuffix));
                updateCommand.Parameters.Add(new OleDbParameter("@lic", dentist.DentistLicenseNumber));
                updateCommand.Parameters.Add(new OleDbParameter("@ptr", dentist.DentistPTRNumber));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                
                Dentist toRemove = DentistList.Single(i => i.DentistID.Equals(dentist.DentistID)); //access the old one
                bool removed = DentistList.Remove(toRemove); //remove it
                if (affectedRows && removed)
                {
                    DentistList.Add(dentist); //add the new one
                    reorderDentistList(); //reorder list
                }
                actionState = affectedRows && removed;
            }
            catch (Exception)
            {
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static bool DeleteDentist(string dentistID)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM " + myTable + " WHERE " + col[0] + "='" + dentistID + "';", GetConnectionObject());
                bool affectedRows = deleteCommand.ExecuteNonQuery() > 0;
                bool accountRemoved = false;
                if (affectedRows) accountRemoved = UserAccountHelper.DeleteUserAccount(dentistID, true); //delete user account also
                bool removed = false;
                if (affectedRows && accountRemoved)
                {
                    Dentist toRemove = DentistList.Single(i => i.DentistID.Equals(dentistID)); //get the item
                    removed = DentistList.Remove(toRemove); //remove the item to list
                }
                actionState = affectedRows && accountRemoved && removed;
            }
            catch (Exception)
            {
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }
        public static void reorderDentistList() //sort the list ascending
        {
            DentistList = new ObservableCollection<Dentist>(DentistList.OrderBy(i => Convert.ToUInt64(i.DentistID))); //sort via ID
        }

    }
}
