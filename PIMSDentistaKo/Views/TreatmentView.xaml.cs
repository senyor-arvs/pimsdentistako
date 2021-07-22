using System.Windows;
using System.Windows.Controls;
using pimsdentistako.DBHelpers;

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
            TreatmentHelper.MyDataGrid = treatmentDataGrid;
            TreatmentTypeHelper.MyDataGrid = treatmentTypeDataGrid;
            TreatmentHelper.InitList();
        }

        private void addTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            /*if (!DatabaseHelper.IsTextBoxTextNullEmpty(treatmentTxtBox))
            {
                _ = TreatmentHelper.AddTreatment(treatmentTxtBox.Text.Trim()); 
            } else
            {
                MessageBox.Show("Please specify the name of the Treatment that you want to add.", "Treatment Adding", MessageBoxButton.OK, MessageBoxImage.Warning);
            }*/
        }

        private void updateTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
           /* var selectedIndex = treatmentDataGrid.SelectedIndex;
            Treatment updatedTreatment = new Treatment
            {
                TreatmentID = TreatmentList[selectedIndex].TreatmentID,
                TreatmentName = treatmentTxtBox.Text
            };
            UpdateTreatment(updatedTreatment, selectedIndex);*/
        }

        private void treatmentDataGrid_SelChanged(object sender, SelectionChangedEventArgs e)
        {
            TreatmentHelper.ListenToDataGrid();
            TreatmentHelper.DisplaySelected(treatmentTxtBox);
            TreatmentTypeHelper.DisplayTypesList();
            treatmentTypeTxtBox.Clear();
        }
        private void treatmentTypeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            treatmentTypeTxtBox.IsReadOnly = false;
            TreatmentTypeHelper.ListenToDataGrid();
            TreatmentTypeHelper.DisplaySelected(treatmentTypeTxtBox);
        }

        private void deleteTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
           /* var selectedIndex = treatmentDataGrid.SelectedIndex;
            DeleteTreatment(TreatmentList[selectedIndex].TreatmentID);
            treatmentTxtBox.Clear();*/
        }

      
    }
}
