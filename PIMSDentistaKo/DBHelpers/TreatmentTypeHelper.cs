using System;
using System.Collections.Generic;

using System.Linq;
using System.Data.OleDb;
using System.Collections.ObjectModel;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using pimsdentistako.DBElements;
using System.Windows.Controls;
using System.Text;

namespace pimsdentistako.DBHelpers
{
    public class TreatmentTypeHelper
    {
        private static int currentSelection;
        private static string myTable = DBNames.TableNames.TREATMENT_TYPE;
        private static string[] col = DBNames.ColumnNames.Treatment_Type;

        public static DataGrid MyDataGrid { get; set; } //data grid 

        public static Dictionary<long, ObservableCollection<TreatmentType>> TreatmentTypeDictionary { get; set; }

        //WORKING
        public static bool InitList(List<string> ID_LIST) //Initialize the List of type based on treatment retreive from database
        {
            bool actionState = false;
            TreatmentTypeDictionary = new Dictionary<long, ObservableCollection<TreatmentType>>();
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);

                OleDbCommand getAllTreatmentTypesCommand = DatabaseHelper.SelectAllCommand(myTable);
                OleDbDataReader dataReader = getAllTreatmentTypesCommand.ExecuteReader();

                ObservableCollection<TreatmentType> typeList = new ObservableCollection<TreatmentType>();
                while (dataReader.Read())
                {
                    TreatmentType type = new TreatmentType
                    {
                        TreatmentTypeID = dataReader[col[0]].ToString(),
                        TreatmentTypeName = dataReader[col[1]].ToString(),
                        TreatmentID = dataReader[col[2]].ToString()
                    };
                    typeList.Add(type);
                }
                //GROUP by ID
                foreach(string id in ID_LIST)
                {
                    ObservableCollection<TreatmentType> filtered = new ObservableCollection<TreatmentType>(typeList.Where(i => i.TreatmentID.Equals(id)));
                    TreatmentTypeDictionary.Add(Convert.ToInt64(id), filtered);
                }
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

        #region DATAGRID LISTENERS AND HANDLERS
        //implement this trategy in all helpers
        //listens to data grid selections - you must add data grid to helper before everything
        public static void ListenToDataGrid()
        {
            currentSelection = MyDataGrid.SelectedIndex;
        }

        //RETURNS the currently selected Dentist based on the inside currentSelection attribute
        public static TreatmentType CurrentlySelectedTreatmentType()
        {
            ObservableCollection<TreatmentType> activeTypeList = TreatmentTypeDictionary[TreatmentHelper.ACTIVE_TREATMENT_ID];
            return DatabaseHelper.IsSelectedIndexValid(currentSelection, activeTypeList.Count) ? activeTypeList[currentSelection] : null;
        }

        //check if handler is grabbing a null dentist
        public static bool IsCurrentlySelectedNull()
        {
            return CurrentlySelectedTreatmentType() == null;
        }

        //Use this for displaying data current selected on DataGrid on the Provided TextBoxes
        public static void DisplaySelected(params TextBox[] textBox)
        {
            
            if (!TreatmentHelper.IsCurrentlySelectedNull())
            {
                ObservableCollection<TreatmentType> activeTypeList = TreatmentTypeDictionary[TreatmentHelper.ACTIVE_TREATMENT_ID];
                if (DatabaseHelper.IsSelectedIndexValid(currentSelection, activeTypeList.Count))
                {
                    for (int i = 0; i < textBox.Length; i++)
                    {
                        textBox[i].Text = CurrentlySelectedTreatmentType().TreatmentTypeName;
                    }
                }
            }
        }
        #endregion DATAGRID LISTENERS AND HANDLERS

        //WORKING
        public static TreatmentType GetLastRecord()
        {
            TreatmentType treatmentType = new TreatmentType();
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand getLastRecordCommand = DatabaseHelper.LastRecordCommand(myTable, col[0]);
                OleDbDataReader dataReader = getLastRecordCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    treatmentType = new TreatmentType
                    {
                        TreatmentTypeID = dataReader[col[0]].ToString(),
                        TreatmentTypeName = dataReader[col[1]].ToString(),
                        TreatmentID = dataReader[col[2]].ToString()
                    };
                }
            } catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);

            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return treatmentType;
        }

        //WORKING
        public static bool AddTreatmentType(TreatmentType treatmentType)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand insertCommand = DatabaseHelper.AddCommand(myTable, col, 1, 2, new List<string>
                {
                    treatmentType.TreatmentTypeName,
                    treatmentType.TreatmentID
                });
                bool affectedRows = insertCommand.ExecuteNonQuery() > 0;
                if (affectedRows)
                {
                    treatmentType = GetLastRecord();
                    //add the treatment to the active treatment collection
                    TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)].Add(treatmentType);
                    actionState = true;
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
        public static bool UpdateTreatmentType(TreatmentType treatmentType)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append("UPDATE ")
                   .Append(myTable)
                   .Append(" SET ")
                   .Append(col[1])
                   .Append("= @treatmentTypeName ")
                   .Append("WHERE ")
                   .Append(col[0])
                   .Append("= @treatmenTypeID");

                OleDbCommand updateCommand = new OleDbCommand(updateQuery.ToString(), GetConnectionObject());

                updateCommand.Parameters.Add(new OleDbParameter("@treatmentTypeName", treatmentType.TreatmentTypeName));
                updateCommand.Parameters.Add(new OleDbParameter("@treatmenTypeID", treatmentType.TreatmentTypeID));

                bool affectedRows = updateCommand.ExecuteNonQuery() > 0;
                if (affectedRows)
                {
                    TreatmentType toBeUpdate = TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)]
                        .FirstOrDefault(i => i.TreatmentTypeID.Equals(treatmentType.TreatmentTypeID));
                    if(toBeUpdate != null)
                    {
                        int position = TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)].IndexOf(toBeUpdate);
                        TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)].Remove(toBeUpdate);
                        TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)].Insert(position, treatmentType);
                    }
                    actionState = true;
                }
            } catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }
            return actionState;
        }

        //WORKING - USED ONLY WHEN DELETING TREATMENT
        public static bool DeleteTreatmentType(string treatmentID)
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand deleteCommand = DatabaseHelper.DeleteCommand(myTable, col[2], treatmentID, false);
                bool affectedRows = deleteCommand.ExecuteNonQuery() > 0;

                if (affectedRows)
                {
                    MyDataGrid.ItemsSource = new ObservableCollection<TreatmentType>(); //place an empty Collection in replacement to currently previewed
                    TreatmentTypeHelper.TreatmentTypeDictionary.Remove(Convert.ToInt64(treatmentID));
                    actionState = affectedRows;
                }
            } catch (Exception e)
            {
                if (DEBUG) DatabaseHelper.DisplayInMessageBox(myTable, e);
            } finally
            {
                requestConnection(ConnectionState.STATE_CLOSE);
            }

            return actionState;
        }

        //TODO UNTESTED DELETE FUNCTION
        public static bool DeleteTreatmentTypeSingle()
        {
            bool actionState = false;
            try
            {
                TreatmentType treatmentType = CurrentlySelectedTreatmentType();
                requestConnection(ConnectionState.STATE_OPEN);
                OleDbCommand deleteCommand = DatabaseHelper.DeleteCommand(myTable, col[0], treatmentType.TreatmentTypeID, true);
                bool affectedRows = deleteCommand.ExecuteNonQuery() > 0;

                if (affectedRows)
                {
                    TreatmentType toBeRemove = TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)]
                        .FirstOrDefault(i => i.TreatmentTypeID.Equals(treatmentType.TreatmentTypeID));
                    if(toBeRemove != null)
                    {
                        TreatmentTypeHelper.TreatmentTypeDictionary[Convert.ToInt64(treatmentType.TreatmentID)].Remove(toBeRemove);
                        actionState = true;
                    }
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

        public static void DisplayTypesList()
        {
            if (TreatmentTypeDictionary.ContainsKey(TreatmentHelper.ACTIVE_TREATMENT_ID))
            {
                MyDataGrid.ItemsSource = TreatmentTypeDictionary[TreatmentHelper.ACTIVE_TREATMENT_ID];
            }
        }
    }
}
