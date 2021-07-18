using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using pimsdentistako.DBElements;
using System.Collections.ObjectModel;
using System.Windows;

namespace pimsdentistako.DBHelpers
{
    public class DatabaseHelper
    {
        //Provider=Microsoft.Jet.OLEDB.4.0;Data Source = C:\mydatabase.mdb;Jet OLEDB:Database Password = MyDbPassword;
        private static OleDbConnection _CONNECTION_OBJ;
        
        public class ConnectionState
        {
            public static readonly string STATE_CLOSE = "Closed";
            public static readonly string STATE_OPEN = "Open";
        }

        public static string GetConnectionState()
        {
            return _CONNECTION_OBJ.State.ToString();
        }

        public static void TestConnection() //use only for testing connection
        {

            _CONNECTION_OBJ.Close(); //will thorw an exception if something is wrong
            MessageBox.Show("Connection State: " + GetConnectionObject().State.ToString());
        }

        public static bool Init() //run on main window
        {
            if (_CONNECTION_OBJ == null)
            {
                _CONNECTION_OBJ = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" +
                    @"Data Source=|DataDirectory|\Database\PIMSDentistaKo.mdb;" +
                    "Jet OLEDB:Database Password = dQXpe}3]?Rx&.7zh*cZ^;");
            }
            return true;
        }

        public static OleDbConnection GetConnectionObject()
        {
            _ = Init();
            return _CONNECTION_OBJ;
        }

        public static void requestConnection(string state)
        {
            _ = Init();
            if (state.Equals(ConnectionState.STATE_OPEN) && GetConnectionState().Equals(ConnectionState.STATE_CLOSE))
            {
                _CONNECTION_OBJ.Open();
            } 
            else if (state.Equals(ConnectionState.STATE_CLOSE) && GetConnectionState().Equals(ConnectionState.STATE_OPEN))
            {
                _CONNECTION_OBJ.Close();
            }
        }

        private static bool compare(string obj1, string obj2)
        {
            return obj1.Equals(obj2);
        }

        public static Int64 GetAvailableID(string Table) //returns 0 if value is DBNull
        {
            string activeIDCol = "";

            if (compare(Table, DBNames.TableNames.DENTAL_REC))
            {
                activeIDCol = DBNames.ColumnNames.Dental_Record[0];
            } 
            else if (compare(Table, DBNames.TableNames.DENTIST))
            { 
                activeIDCol = DBNames.ColumnNames.Dentist[0];
            }
            else if (compare(Table, DBNames.TableNames.EMERGENCY_INFO))
            {
                activeIDCol = DBNames.ColumnNames.Emergency_Information[0];
            }
            else if (compare(Table, DBNames.TableNames.PATIENT))
            {
                activeIDCol = DBNames.ColumnNames.Patient[0];
            }
            else if (compare(Table, DBNames.TableNames.TREATMENT))
            {
                activeIDCol = DBNames.ColumnNames.Treatment[0];
            }
            else if (compare(Table, DBNames.TableNames.TREATMENT_PLAN))
            {
                activeIDCol = DBNames.ColumnNames.Treatment_Plan[0];
            }
            else if (compare(Table, DBNames.TableNames.TREATMENT_TYPE))
            {
                activeIDCol = DBNames.ColumnNames.Treatment_Type[0];
            }
            else if (compare(Table, DBNames.TableNames.USER_ACC))
            {
                activeIDCol = DBNames.ColumnNames.User_Account[0];
            }

            requestConnection(ConnectionState.STATE_OPEN);
            OleDbCommand maxIdCommand = new OleDbCommand("SELECT MAX(" + activeIDCol + ") FROM " + Table, GetConnectionObject());
            object obj = maxIdCommand.ExecuteScalar();
            Int32 maxAvailble = -1;
            if (obj != null && DBNull.Value != obj)
            {
                maxAvailble = (Int32)obj;
            }
            maxAvailble += 1;
            return maxAvailble;
        }
    }
}
