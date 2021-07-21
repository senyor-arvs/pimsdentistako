using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using dentis_Add_Edit_Delete.DBElements;
using dentis_Add_Edit_Delete.DBHelpers;

namespace dentis_Add_Edit_Delete
{
    /// <summary>
    /// Interaction logic for deleteWindow.xaml
    /// </summary>
    public partial class deleteWindow : Window
    {
        private Dentist active;
        public deleteWindow()
        {
            InitializeComponent();
            active = DentistHelper.CurrentlySelectedDentist();
            labelConfirmation.Content = "Are you sure you want to delete:\n" + active.DentistFullName + "?";
        }

        private void btnDeleteConfirm_Click(object sender, RoutedEventArgs e)
        {
            bool actionResult = DentistHelper.DeleteDentist(active.DentistID);
            if (actionResult)
            {
                MessageBox.Show("Dentist was Successfuly Deleted.");
            }
            else
            {
                MessageBox.Show("Ann error occured while deleting Dentist.\nNothing was deleted");
            }
            this.Close();
        }

        private void btnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
