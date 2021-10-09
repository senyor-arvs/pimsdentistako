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
                treatmentIDS = new List<string>();
                OleDbCommand getAllTreatmentCommand = DatabaseHelper.SelectAllCommand(myTable);
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

        public static Treatment GetLastRecord()
        {
            Treatment treatment = new Treatment();
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand getLastRecordCommand = DatabaseHelper.LastRecordCommand(myTable, col[0]);
                OleDbDataReader dataReader = getLastRecordCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    treatment = new Treatment
                    {
                        TreatmentID = dataReader[col[0]].ToString(),
                        TreatmentName = dataReader[col[1]].ToString()
                    };
                }
            } catch(Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return treatment;
        }

        //WORKING
        public static bool AddTreatment(String treatmentName)
        {
            bool actionState = false;
            Treatment treatment;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO ").Append(myTable).Append(" (")
                    .Append(col[1]).Append(") ")
                    .Append(" VALUES (@treatmentName)");

                OleDbCommand insertCommand = new OleDbCommand(query.ToString(), GetConnectionObject());

                insertCommand.Parameters.Add(new OleDbParameter("@treatmentName", treatmentName));

                bool initial_adding = insertCommand.ExecuteNonQuery() > 0;
                if (initial_adding)
                {
                    treatment = GetLastRecord();
                    TreatmentList.Add(treatment);

                    //add id to treatment type dictionary and an empty collection
                    TreatmentTypeHelper.TreatmentTypeDictionary.Add(
                        Convert.ToInt64(treatment.TreatmentID), 
                        new ObservableCollection<TreatmentType>());
                    actionState = true;
                }
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

        //WORKING
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
                if (affectedRows)
                {
                    TreatmentList[selectedIndex] = treatment;
                    actionState = affectedRows;
                }
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

        //WORKING
        public static bool DeleteTreatment(string treatmentID)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand deleteCommand = DatabaseHelper.DeleteCommand(myTable, col[0], treatmentID, true);
                bool affectedRows = deleteCommand.ExecuteNonQuery() > 0;

                if(affectedRows)
                {
                    Treatment toRemove = TreatmentList.Single(i => i.TreatmentID.Equals(treatmentID)); //get the item
                    string removedID = toRemove.TreatmentID;

                    //use the size of the treatment observable collection as an indicator if there are type exist in database.
                    int numberOfTypeIntanceExist = TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(removedID)].Count;
                    if (numberOfTypeIntanceExist > 0)
                    {
                        //perform removal to database
                        if(TreatmentTypeHelper.DeleteTreatmentType(removedID))                        
                            actionState = TreatmentList.Remove(toRemove); //remove the item to list
                    } else
                    {
                        actionState = TreatmentList.Remove(toRemove); //remove the item to list
                    }
                }
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
