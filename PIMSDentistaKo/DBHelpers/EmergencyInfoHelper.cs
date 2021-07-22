using System;
using System.Text;

using System.Data.OleDb;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;

namespace pimsdentistako.DBHelpers
{
    //THIS HELPER IS UNFINISHED SINCE MANY FUNCTIONS ARE NOT IMPLEMENTED YET
    public class EmergencyInfoHelper
    {
        private static string myTable = DBNames.TableNames.EMERGENCY_INFO;
        private static string[] col = DBNames.ColumnNames.Emergency_Information;

        //this class was actually used on the feature of adding office number when
        //it is the office number is the same as the parent patient
        //since this feature is unimplemented
        //I will set the contactOfficeNumberRemarks to UNIQUE_TO_PATIENT in all code paths
        public class OfficeNumberRemarks
        {
            public static readonly string SAME_TO_PATIENT = "same to patient";
            public static readonly string UNIQUE_TO_PATIENT = "unique to patient";
        }

        public static bool AddEmergencyInfo(string patientID, EmergencyInfo emergencyInfo)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO ").Append(myTable).Append(" (")
                    .Append(col[1]).Append(", ")
                    .Append(col[2]).Append(", ")
                    .Append(col[3]).Append(", ")
                    .Append(col[4]).Append(", ")
                    .Append(col[5]).Append(", ")
                    .Append(col[6]).Append(") ")
                    .Append(" VALUES ").Append(" (")
                    .Append("@patientID").Append(", ")
                    .Append("@contactName").Append(", ")
                    .Append("@contactNumber").Append(", ")
                    .Append("@conOfficeNum").Append(", ")
                    .Append("@relationship").Append(", ")
                    .Append("@remarks").Append(")");

                OleDbCommand insertCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                insertCommand.Parameters.Add(new OleDbParameter("@patientID", patientID));
                insertCommand.Parameters.Add(new OleDbParameter("@contactName", emergencyInfo.ContactName));
                insertCommand.Parameters.Add(new OleDbParameter("@contactNumber", emergencyInfo.ContactNumber));
                insertCommand.Parameters.Add(new OleDbParameter("@conOfficeNum", emergencyInfo.ContactOfficeNumber));
                insertCommand.Parameters.Add(new OleDbParameter("@relationship", emergencyInfo.ContactRelationship));
                insertCommand.Parameters.Add(new OleDbParameter("@remarks", emergencyInfo.ContactOfficeNumberRemarks));

                actionState = insertCommand.ExecuteNonQuery() > 0;
            } catch(Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }
    }
}
