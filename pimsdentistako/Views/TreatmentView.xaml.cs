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
    public partial class TreatmentView : UserControl, IWindowCloseListener, IOnDeleteSuccessListener
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
            AddEditTreatmentWindow addEditTreatmentWindow = new AddEditTreatmentWindow(mode: 0);
            addEditTreatmentWindow.WindowCloseListener = this;
            addEditTreatmentWindow.Show();
        }

        private void updateTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TreatmentHelper.IsCurrentlySelectedNull())
            {
                AddEditTreatmentWindow addEditTreatmentWindow = new AddEditTreatmentWindow(
                   mode: 1,
                   treatmentName: treatmentTxtBox.Text,
                   dataGrid_SelectedIndex: treatmentDataGrid.SelectedIndex
                );
                addEditTreatmentWindow.WindowCloseListener = this;
                addEditTreatmentWindow.Show();
            } else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_TREATMENT_SELECTION);
            }
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
            //TODO IMPLEMENT CONFIRMATION DIALOG
            if (!TreatmentHelper.IsCurrentlySelectedNull())
            {
                DeleteConfimationWindow deleteConfirmation = new DeleteConfimationWindow(DeleteConfimationWindow.DeleteWindowModes.TREATMENT_MODE);
                deleteConfirmation.WindowCloseListener = this;
                deleteConfirmation.OnDeleteSuccessListener = this;
                deleteConfirmation.Show();
            } else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_TREATMENT_SELECTION);
            }
        }

        public void OnWindowClose(bool isClosed)
        {
            Window.GetWindow(this).IsEnabled = isClosed;
        }

        private void addTreatmentTypeButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditTreatmentTypeWindow addEditTreatmentTypeWindow = new AddEditTreatmentTypeWindow(0, TreatmentHelper.CurrentlySelectedTreatment());
            addEditTreatmentTypeWindow.WindowCloseListener = this;
            addEditTreatmentTypeWindow.Show();
        }

        private void updateTreatmentTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TreatmentTypeHelper.IsCurrentlySelectedNull())
            {
                AddEditTreatmentTypeWindow addEditTreatmentTypeWindow = new AddEditTreatmentTypeWindow(1, TreatmentHelper.CurrentlySelectedTreatment());
                addEditTreatmentTypeWindow.WindowCloseListener = this;
                addEditTreatmentTypeWindow.Show();
            } else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_TREATMENT_TYPE_SELECTION);
            }

        }

        private void deleteTreatmentTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TreatmentTypeHelper.IsCurrentlySelectedNull())
            {
                DeleteConfimationWindow deleteConfirmation = new DeleteConfimationWindow(DeleteConfimationWindow.DeleteWindowModes.TREATMENT_TYPE_MODE);
                deleteConfirmation.WindowCloseListener = this;
                deleteConfirmation.OnDeleteSuccessListener = this;
                deleteConfirmation.Show();
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_TREATMENT_TYPE_SELECTION);
            }
        }

        public void OnDeleteSuccess(bool deleted, string owner)
        {
            if (deleted)
            {
                if (owner == DeleteConfimationWindow.DeleteWindowTitles.TREATMENT_MODE) treatmentTxtBox.Clear();
                else if (owner == DeleteConfimationWindow.DeleteWindowTitles.TREATMENT_TYPE_MODE) treatmentTypeTxtBox.Clear();
            }
        }
    }
}
