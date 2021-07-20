using pimsdentistako.DBElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static pimsdentistako.DBHelpers.TreatmentHelper;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for TreatmentView.xaml
    /// </summary>
    public partial class TreatmentView : UserControl
    {
        public TreatmentView()
        {
            InitializeComponent();
            InitList();
            treatmentDataGrid.ItemsSource = TreatmentList;
        }

        private void addTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            _ = AddTreatment(treatmentTxtBox.Text.Trim());
        }

        private void updateTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = treatmentDataGrid.SelectedIndex;
            Treatment updatedTreatment = new Treatment
            {
                TreatmentID = TreatmentList[selectedIndex].TreatmentID,
                TreatmentName = treatmentTxtBox.Text
            };
            UpdateTreatment(updatedTreatment, selectedIndex);
        }

        private void treatmentDataGrid_SelChanged(object sender, SelectionChangedEventArgs e)
        {
            treatmentTxtBox.IsReadOnly = false;
            try
            {
                var data = treatmentDataGrid.SelectedItem;
                string treatmentName = (treatmentDataGrid.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                treatmentTxtBox.Text = treatmentName;
            }
            catch (Exception)
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                        if (!dgr.IsMouseOver)
                        {
                            (dgr as DataGridRow).IsSelected = false;
                        }
                    }
                }
            }
        }

        private void deleteTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = treatmentDataGrid.SelectedIndex;
            DeleteTreatment(TreatmentList[selectedIndex].TreatmentID);
            treatmentTxtBox.Clear();
        }
    }
}
