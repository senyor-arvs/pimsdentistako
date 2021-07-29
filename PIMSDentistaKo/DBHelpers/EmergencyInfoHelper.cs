using System;
using System.Text;

using System.Data.OleDb;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using System.Collections.Generic;

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

        //WORKING
        public static bool AddEmergencyInfo(string patientID, EmergencyInfo emergencyInfo)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand insertCommand = DatabaseHelper.AddCommand(myTable, col, 1, 6, new List<string>
                {
                    patientID,
                    emergencyInfo.ContactName,
                    emergencyInfo.ContactNumber,
                    emergencyInfo.ContactOfficeNumber,
                    emergencyInfo.ContactRelationship,
                    emergencyInfo.ContactOfficeNumberRemarks
                });

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

        //WORKING
        public static EmergencyInfo RetrievePatientEmergencyInfo(string patientID)
        {
            EmergencyInfo emergencyInfo = new EmergencyInfo();
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand getCommand = new OleDbCommand("SELECT * FROM " + myTable + " WHERE " + col[1] + "='" + patientID + "'", GetConnectionObject());
                OleDbDataReader reader = getCommand.ExecuteReader();

                while (reader.Read())
                {
                    emergencyInfo.ContactID = reader[col[0]].ToString();
                    emergencyInfo.PatientID = reader[col[1]].ToString();
                    emergencyInfo.ContactName = reader[col[2]].ToString();
                    emergencyInfo.ContactNumber = reader[col[3]].ToString();
                    emergencyInfo.ContactOfficeNumber = reader[col[4]].ToString();
                    emergencyInfo.ContactRelationship = reader[col[5]].ToString();
                    emergencyInfo.ContactOfficeNumberRemarks = reader[col[6]].ToString();
                }
            } catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return emergencyInfo;
        }

        //WORKING
        //Automatically Called Inside PatientHelper Update Method
        public static bool UpdateEmergencyInformation(EmergencyInfo emergencyInfo)
        {
            bool actionState = false;

            try
            {
                if (!IsEmergencyInformationExist(emergencyInfo.PatientID))
                {
                    //if Not Existing Add it
                    actionState = AddEmergencyInfo(emergencyInfo.PatientID, emergencyInfo);
                } else
                {
                    requestConnection(ConnectionState.STATE_OPEN);
                    //if exisiting update it
                    StringBuilder query = new StringBuilder(0);
                    query.Append(" UPDATE ").Append(myTable).Append(" SET ")
                        .Append(col[2]).Append("= ?, ")
                        .Append(col[3]).Append("= ?, ")
                        .Append(col[5]).Append("= ?, ")
                        .Append(col[4]).Append("= ?, ")
                        .Append(col[6]).Append("= ? ")
                        .Append("WHERE ").Append(col[1]).Append("= ?");

                    OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                    updateCommand.Parameters.Add(new OleDbParameter("@name", emergencyInfo.ContactName));
                    updateCommand.Parameters.Add(new OleDbParameter("@number", emergencyInfo.ContactNumber));
                    updateCommand.Parameters.Add(new OleDbParameter("@relationship", emergencyInfo.ContactRelationship));
                    updateCommand.Parameters.Add(new OleDbParameter("@officeNumber", emergencyInfo.ContactOfficeNumber));
                    updateCommand.Parameters.Add(new OleDbParameter("@remarks", emergencyInfo.ContactOfficeNumberRemarks));
                    updateCommand.Parameters.Add(new OleDbParameter("@id", emergencyInfo.PatientID));

                    actionState = updateCommand.ExecuteNonQuery() > 0;
                }
            } catch(Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }

            return actionState;
        }

        //WORKING
        public static bool IsEmergencyInformationExist(string patientID)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand selectCommand = DatabaseHelper.SelectAllThatSatisfiesCommand(myTable, col[1], patientID, false);
                OleDbDataReader dataReader = selectCommand.ExecuteReader();

                actionState = dataReader.HasRows;
            } catch(Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }
    }
}
