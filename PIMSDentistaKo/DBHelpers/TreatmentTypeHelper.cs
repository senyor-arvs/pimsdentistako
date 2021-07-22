using System;
using System.Collections.Generic;

using System.Linq;
using System.Data.OleDb;
using System.Collections.ObjectModel;
using static pimsdentistako.DBHelpers.DatabaseHelper;
using pimsdentistako.DBElements;
using System.Windows.Controls;

namespace pimsdentistako.DBHelpers
{
    public class TreatmentTypeHelper
    {
        private static int currentSelection;
        private static string myTable = DBNames.TableNames.TREATMENT_TYPE;
        private static string[] col = DBNames.ColumnNames.Treatment_Type;

        public static DataGrid MyDataGrid { get; set; } //data grid 

        private static Dictionary<long, ObservableCollection<TreatmentType>> treatmentTypeDictionary;

        public static Dictionary<long, ObservableCollection<TreatmentType>> TreatmentTypeDictionary { get => treatmentTypeDictionary; set => treatmentTypeDictionary = value; }

        //WORKING
        public static bool InitList(List<string> ID_LIST) //Initialize the List of patients retreive from database
        {
            bool actionState = false;
            try
            {
                requestConnection(ConnectionState.STATE_OPEN);
                treatmentTypeDictionary = new Dictionary<long, ObservableCollection<TreatmentType>>();

                OleDbCommand getAllTreatmentTypesCommand = new OleDbCommand("SELECT * FROM " + myTable, GetConnectionObject());
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
                    treatmentTypeDictionary.Add(Convert.ToInt64(id), filtered);
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
            ObservableCollection<TreatmentType> activeTypeList = treatmentTypeDictionary[TreatmentHelper.ACTIVE_TREATMENT_ID];
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
                ObservableCollection<TreatmentType> activeTypeList = treatmentTypeDictionary[TreatmentHelper.ACTIVE_TREATMENT_ID];
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

        public static void DisplayTypesList()
        {
            MyDataGrid.ItemsSource = treatmentTypeDictionary[TreatmentHelper.ACTIVE_TREATMENT_ID];
        }
    }
}
