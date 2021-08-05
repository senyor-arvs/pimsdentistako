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
                    _ = TreatmentHelper.AddTreatment(treatmentNameTxtBox.Text.Trim());
                    Close();
                }
                else
                {
                    MessageBox.Show("Please specify the name of the Treatment that you want to add.", "Treatment Adding", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            // If Mode is set to EDIT TREATMENT
            else if (mode == 1) 
            {
                if (treatmentNameTxtBox.Text == "")
                {
                    MessageBox.Show("Null treatment name not allowed.", "Treatment Editing", MessageBoxButton.OK, MessageBoxImage.Warning);
                } 
                else
                {
                    var selectedIndex = dataGrid_SelectedIndex;
                    Treatment updatedTreatment = new Treatment
                    {
                        TreatmentID = TreatmentHelper.TreatmentList[selectedIndex].TreatmentID,
                        TreatmentName = treatmentNameTxtBox.Text.Trim()
                    };
                    TreatmentHelper.UpdateTreatment(updatedTreatment, selectedIndex);
                    Close();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
