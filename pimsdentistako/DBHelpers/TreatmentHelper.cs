using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using pimsdentistako.DBElements;
using static pimsdentistako.DBHelpers.DatabaseHelper;

namespace pimsdentistako.DBHelpers
{
    public class TreatmentHelper
    {
        public static long ACTIVE_TREATMENT_ID;
        private static int currentSelection;
        public static List<string> treatmentIDS = new List<string>();
        private static readonly string myTable = DBNames.TableNames.TREATMENT;
        private static readonly string[] col = DBNames.ColumnNames.Treatment;

        public static DataGrid MyDataGrid { get; set; } //data grid 

        public static ObservableCollection<Treatment> TreatmentList { get; set; }
        public static bool InitList() //INTIALIZE THE LIST OF TREATMENTS
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
                    treatmentIDS.Add(treatment.TreatmentID);
                }
                TreatmentTypeHelper.InitList(treatmentIDS); //initialize the list of treatment types
                reorderTreatmentList();
            }
            catch (OleDbException e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
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
                    if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
                }

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
            if (MyDataGrid != null) MyDataGrid.ItemsSource = TreatmentList;
        }

        #region DATAGRID LISTENERS AND HANDLERS
        //implement this trategy in all helpers
        //listens to data grid selections - you must add data grid to helper before everything
        public static void ListenToDataGrid()
        {
            currentSelection = MyDataGrid.SelectedIndex;
        }

        //RETURNS the currently selected Dentist based on the inside currentSelection attribute
        public static Treatment CurrentlySelectedTreatment()
        {
            return DatabaseHelper.IsSelectedIndexValid(currentSelection, TreatmentList.Count) ? TreatmentList[currentSelection] : null;
        }

        //check if handler is grabbing a null dentist
        public static bool IsCurrentlySelectedNull()
        {
            return CurrentlySelectedTreatment() == null;
        }

        //Use this for displaying data current selected on DataGrid on the Provided TextBoxes
        public static void DisplaySelected(params TextBox[] textBox)
        {
            if (DatabaseHelper.IsSelectedIndexValid(currentSelection, TreatmentList.Count))
            {
                ACTIVE_TREATMENT_ID = Convert.ToInt64(CurrentlySelectedTreatment().TreatmentID);
                for (int i = 0; i < textBox.Length; i++)
                {
                    textBox[i].Text = CurrentlySelectedTreatment().TreatmentName;
                }
            }
        }
        #endregion DATAGRID LISTENERS AND HANDLERS
    }
}
