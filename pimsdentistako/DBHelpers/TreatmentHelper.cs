using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;

namespace pimsdentistako.DBHelpers
{
    public class TreatmentHelper
    {

        private static readonly string myTable = DBNames.TableNames.TREATMENT;
        private static readonly string[] col = DBNames.ColumnNames.Treatment;

        public static ObservableCollection<Treatment> TreatmentList { get; set; }
        public static bool InitList() //RUN ON DENTIST VIEW BEFORE GETTING THE DENTIST OBJECT - This will also refresh the List
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                TreatmentList = new ObservableCollection<Treatment>();
                OleDbCommand getAllTreatmentCommand = new OleDbCommand("SELECT * FROM " + myTable, GetConnectionObject());
                OleDbDataReader dataReader = getAllTreatmentCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    Treatment treatment = new Treatment
                    {
                        TreatmentID = dataReader[col[0]].ToString(),
                        TreatmentName = dataReader[col[1]].ToString(),
                    };

                    TreatmentList.Add(treatment);
                }
                reorderTreatmentList();
                actionState = true;
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static bool AddTreatment(String treatmentName)
        {
            int maxAvailable;
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                // Get max number of ID
                OleDbCommand getMaxID = new OleDbCommand("SELECT MAX(" + col[0] + ") from " + myTable, GetConnectionObject());
                try
                {
                    maxAvailable = (int)getMaxID.ExecuteScalar();
                    maxAvailable += 1;
                }
                catch (InvalidCastException e)
                {
                    maxAvailable = 0;
                }

                MessageBox.Show(maxAvailable.ToString());

                Treatment treatment = new Treatment
                {
                    TreatmentID = maxAvailable.ToString(),
                    TreatmentName = treatmentName
                };

                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO ").Append(myTable).Append(" (")
                    .Append(col[1]).Append(") ")
                    .Append(" VALUES (@treatmentName)");

                OleDbCommand insertCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                insertCommand.Parameters.Add(new OleDbParameter("@treatmentName", treatmentName));

                bool initial_adding = insertCommand.ExecuteNonQuery() > 0;
                actionState = true;

                TreatmentList.Add(treatment);
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static bool UpdateTreatment(Treatment treatment, int selectedIndex)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("UPDATE ")
                    .Append(myTable)
                    .Append(" SET ")
                    .Append(col[1])
                    .Append("= @treatmentName ")
                    .Append("WHERE ")
                    .Append(col[0])
                    .Append("= @treatmentID");

                OleDbCommand updateCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@treatmentName", treatment.TreatmentName));
                updateCommand.Parameters.Add(new OleDbParameter("@treatmentID", treatment.TreatmentID));
                
                

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;

                TreatmentList[selectedIndex] = treatment;

            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static bool DeleteTreatment(string treatmentID)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM " + myTable + " WHERE " + col[0] + "=" + treatmentID + ";", GetConnectionObject());
                bool affectedRows = deleteCommand.ExecuteNonQuery() > 0;

                Treatment toRemove = TreatmentList.Single(i => i.TreatmentID.Equals(treatmentID)); //get the item
                TreatmentList.Remove(toRemove); //remove the item to list
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        public static void reorderTreatmentList() //sort the list ascending
        {
            TreatmentList = new ObservableCollection<Treatment>(TreatmentList.OrderBy(i => Convert.ToUInt64(i.TreatmentID))); //sort via ID
        }
    }
}
