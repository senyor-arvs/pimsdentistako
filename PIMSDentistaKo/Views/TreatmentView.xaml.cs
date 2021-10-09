using System.Windows;
using System.Windows.Controls;
using pimsdentistako.DBHelpers;
using pimsdentistako.Windows;
using pimsdentistako.Callbacks;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for TreatmentView.xaml
    /// </summary>
    public partial class TreatmentView : UserControl, IWindowCloseListener
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
            AddEditTreatmentWindow addEditTreatmentWindow = new AddEditTreatmentWindow(mode: 0);
            addEditTreatmentWindow.WindowCloseListener = this;
            addEditTreatmentWindow.Show();
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
            AddEditTreatmentWindow addEditTreatmentWindow = new AddEditTreatmentWindow(
                mode: 1,
                treatmentName: treatmentTxtBox.Text,
                dataGrid_SelectedIndex: treatmentDataGrid.SelectedIndex
            );
            addEditTreatmentWindow.WindowCloseListener = this;
            addEditTreatmentWindow.Show();
        }

        private void treatmentDataGrid_SelChanged(object sender, SelectionChangedEventArgs e)
        {
            TreatmentHelper.ListenToDataGrid();
            TreatmentHelper.DisplaySelected(treatmentTxtBox);
            TreatmentTypeHelper.DisplayTypesList();
            treatmentTypeTxtBox.Clear();
            updateTreatmentButton.Visibility = Visibility.Visible;
            deleteTreatmentButton.Visibility = Visibility.Visible;
            addTreatmentTypeButton.Visibility = Visibility.Visible;
        }
        private void treatmentTypeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            treatmentTypeTxtBox.IsReadOnly = false;
            TreatmentTypeHelper.ListenToDataGrid();
            TreatmentTypeHelper.DisplaySelected(treatmentTypeTxtBox);
            updateTreatmentTypeButton.Visibility = Visibility.Visible;
            deleteTreatmentTypeButton.Visibility = Visibility.Visible;
        }

        private void deleteTreatmentButton_Click(object sender, RoutedEventArgs e)
        {

           var selectedIndex = treatmentDataGrid.SelectedIndex;
           TreatmentHelper.DeleteTreatment(TreatmentHelper.TreatmentList[selectedIndex].TreatmentID);
           treatmentTxtBox.Clear(); 
        }

        public void OnWindowClose(bool isClosed)
        {
            Window.GetWindow(this).IsEnabled = isClosed;
        }
    }
}
