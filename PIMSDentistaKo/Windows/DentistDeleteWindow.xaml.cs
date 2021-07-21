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

using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for DentistDeleteWindow.xaml
    /// </summary>
    public partial class DentistDeleteWindow : Window
    {
        private Dentist active;
        public DentistDeleteWindow()
        {
            InitializeComponent();
            active = DentistHelper.CurrentlySelectedDentist();
            labelConfirmation.Text = "Are you sure you want to delete \n" + active.DentistFullName + "?";
        }

        private void Cancel_btn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            bool actionResult = DentistHelper.DeleteDentist(active.DentistID);
            if (actionResult)
            {
                DatabaseHelper.DisplayDialog("Deleting Dentist", "Dentist was Successfuly Deleted.");
            }
            else
            {
                DatabaseHelper.DisplayErrorDialog("Deleting Dentist", "An error occured while deleting Dentist.\nNothing was deleted");
            }
            this.Close();
        }
    }
}
