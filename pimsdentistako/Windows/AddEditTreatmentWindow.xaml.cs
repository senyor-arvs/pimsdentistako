using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;
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

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for AddEditTreatmentWindow.xaml
    /// </summary>
    public partial class AddEditTreatmentWindow : Window
    {
        private int mode;
        private string treatmentName;
        private int dataGrid_SelectedIndex;

        public IWindowCloseListener WindowCloseListener { get; set; }

        public AddEditTreatmentWindow(int mode, string treatmentName = "", int dataGrid_SelectedIndex = 0)
        {
            this.mode = mode;
            this.treatmentName = treatmentName;
            this.dataGrid_SelectedIndex = dataGrid_SelectedIndex;
            InitializeComponent();
            titleTxt.Text = (mode == 0) ? "Add Treatment" : "Edit Treatment";
            treatmentNameTxtBox.Text = treatmentName;
            addEditButton.Content = (mode == 0) ? "Add" : "Edit";
        }

        private void addEditButton_Click(object sender, RoutedEventArgs e)
        {
            // If Mode is set to ADD TREATMENT
            if (mode == 0)
            {
                if (!DatabaseHelper.IsTextBoxTextNullEmpty(treatmentNameTxtBox))
                {
                    bool actionResult = TreatmentHelper.AddTreatment(treatmentNameTxtBox.Text.Trim());
                    if (actionResult)
                    {
                        DatabaseHelper.DisplayDialog("Adding Treatment", "Treatement was added successfully.");
                        Close();
                    }
                    else
                    {
                        DatabaseHelper.DisplayErrorDialog(Errors.ErrorCodes.ERR_FAILED_ADDING_TREATMENT);
                    }
                }
                else
                {
                    DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_UNSPECIFIED_TREATMENT);
                }
            }

            // If Mode is set to EDIT TREATMENT
            else if (mode == 1) 
            {
                if (DatabaseHelper.IsTextBoxTextNullEmpty(treatmentNameTxtBox))
                {
                    DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_UNSPECIFIED_TREATMENT_EDITING);
                } 
                else
                {
                    var selectedIndex = dataGrid_SelectedIndex;
                    Treatment updatedTreatment = new Treatment
                    {
                        TreatmentID = TreatmentHelper.TreatmentList[selectedIndex].TreatmentID,
                        TreatmentName = treatmentNameTxtBox.Text.Trim()
                    };
                    bool actionResult = TreatmentHelper.UpdateTreatment(updatedTreatment, selectedIndex);
                    if (actionResult)
                    {
                        DatabaseHelper.DisplayDialog("Updating Treatment", "The selected Treatment was updated successfully.");
                        Close();
                    } else
                    {
                        DatabaseHelper.DisplayErrorDialog(Errors.ErrorCodes.ERR_FAILED_ADDING_TREATMENT);
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        

        private void Window_Closed(object sender, EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }
    }
}
