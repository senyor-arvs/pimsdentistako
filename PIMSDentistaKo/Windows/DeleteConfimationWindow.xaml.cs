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
using pimsdentistako.DBHelpers;
using pimsdentistako.DBElements;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for DeleteConfimationWindow.xaml
    /// </summary>
    public partial class DeleteConfimationWindow : Window
    {
        public static class DeleteWindowModes
        {
            public static int DENTIST_MODE = 0;
            public static int PATIENT_MODE = 1;
        }

        public static class DeleteWindowTitles
        {
            public static string DENTIST_MODE = "Deleting Dentist Confirmation";
            public static string PATIENT_MODE = "Deleting Patient Confirmation";
        }

        public IWindowCloseListener WindowCloseListener { get; set; }
        private string displayName = DatabaseHelper.BLANK_INPUT;
        private string windowTitle = DatabaseHelper.BLANK_INPUT;
        private readonly int mode;

        public DeleteConfimationWindow(int mode)
        {
            this.mode = mode;
            InitializeComponent();
            initialConfiguration();
        }

        private void initialConfiguration()
        {
            if (mode == DeleteWindowModes.DENTIST_MODE)
            {
                displayName = DentistHelper.CurrentlySelectedDentist().DentistFullName;
                windowTitle = DeleteWindowTitles.DENTIST_MODE;
            }
            else if (mode == DeleteWindowModes.PATIENT_MODE)
            {
                displayName = PatientHelper.CurrentlySelectedPatient().PatientFullName;
                windowTitle = DeleteWindowTitles.PATIENT_MODE;
            }
            deleteConfirmationWindowMain.Title = windowTitle;
            labelConfirmation.Text = "Are you sure you want to delete \n" + displayName + "?";
        }

        private void Confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            if(mode == DeleteWindowModes.DENTIST_MODE)
            {
                Dentist dentist = DentistHelper.CurrentlySelectedDentist();
                bool actionResult = DentistHelper.DeleteDentist(dentist.DentistID);
                if (actionResult)
                {
                    DatabaseHelper.DisplayDialog("Deleting Dentist", "Dentist was Successfuly Deleted.");
                }
                else
                {
                    DatabaseHelper.DisplayErrorDialog("Deleting Dentist", "An error occured while deleting Dentist.\nNothing was deleted");
                }

            } else if(mode == DeleteWindowModes.PATIENT_MODE)
            {
                //TODO: DELETE PATIENT COMMAND HERE
                Patient patient = PatientHelper.CurrentlySelectedPatient();
            }
            this.Close();
        }

        private void Cancel_btn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }
    }
}
