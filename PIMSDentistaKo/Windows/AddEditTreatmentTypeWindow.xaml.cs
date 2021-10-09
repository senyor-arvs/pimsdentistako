using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using pimsdentistako.Callbacks;
using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for AddEditTreatmentTypeWindow.xaml
    /// </summary>
    public partial class AddEditTreatmentTypeWindow : Window
    {
        public IWindowCloseListener WindowCloseListener { get; set; }
        private Treatment treatment;
        private readonly int mode;

        public AddEditTreatmentTypeWindow(int mode, Treatment treatment)
        {
            this.treatment = treatment;
            this.mode = mode;
            InitializeComponent();
            if(mode == 0)
            {
                titleTxt.Text = "Add Treatment Type";
                addEditButton.Content = "ADD";
            } else
            {
                titleTxt.Text = "Edit Treatment Type";
                addEditButton.Content = "UPDATE";
                treatmentTypeTxtBox.Text = TreatmentTypeHelper.CurrentlySelectedTreatmentType().TreatmentTypeName;
            }
            treatmentTxt.Text = this.treatment.TreatmentName;
        }

        private void AddTreatmentType()
        {
            if (!DatabaseHelper.IsTextBoxTextNullEmpty(treatmentTypeTxtBox))
            {
                TreatmentType treatmentType = new TreatmentType
                {
                    TreatmentID = this.treatment.TreatmentID,
                    TreatmentTypeName = treatmentTypeTxtBox.Text.Trim()
                };
                //TODO ADD ERROR HANDLING IF FUNCTION RETURNS FALSE
                bool success = TreatmentTypeHelper.AddTreatmentType(treatmentType);
                if (success)
                {
                    DatabaseHelper.DisplayDialog("Adding Treatment Type", "The Treatment Type was successfully added.");
                    this.Close();
                }
                else
                {
                    DatabaseHelper.DisplayErrorDialog(Errors.ErrorCodes.ERR_FAILED_ADDING_TREATMENT_TYPE);
                }
            } else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_UNSPECIFIED_TREATMENT_TYPE);
            }
        }

        private void EditTreatmentType()
        {
            if (!DatabaseHelper.IsTextBoxTextNullEmpty(treatmentTypeTxtBox))
            {
                TreatmentType updatedTreatmentType = new TreatmentType
                {
                    TreatmentTypeID = TreatmentTypeHelper.CurrentlySelectedTreatmentType().TreatmentTypeID,
                    TreatmentTypeName = treatmentTypeTxtBox.Text.Trim(), //replaced named
                    TreatmentID = TreatmentTypeHelper.CurrentlySelectedTreatmentType().TreatmentID
                };
                if (TreatmentTypeHelper.UpdateTreatmentType(updatedTreatmentType))
                {
                    DatabaseHelper.DisplayDialog("Updating Treatment Type", "The selected Treatment Type was successfully updated.");
                    Close();
                } else
                {
                    DatabaseHelper.DisplayErrorDialog(Errors.ErrorCodes.ERR_FAILED_ADDING_TREATMENT_TYPE);
                }
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_UNSPECIFIED_TREATMENT_TYPE_EDITING);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.mode == 0) AddTreatmentType();
            else EditTreatmentType();
        }
    }
}
