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
using System.Windows.Navigation;
using System.Windows.Shapes;
using pimsdentistako.ViewModels;
using pimsdentistako.Views;
using pimsdentistako.DBHelpers;
using pimsdentistako.DBElements;
using System.Collections.ObjectModel;

namespace pimsdentistako
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool pressed = false;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(DateTime.Today);
            Mainscreen.Height = 1080 * 0.95;
            Mainscreen.Width = 1920 * 0.95;
            //this.WindowState = WindowState.Maximized;
            DatabaseHelper.Init();
        }
        private void ProfilePicture_LeftClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hello!");
        }

        private void transactionsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentsButton.IsEnabled && PatientsButton.IsEnabled)
            {
                AppointmentsButton.IsEnabled = false; // drop down menu item
                PatientsButton.IsEnabled = false; // drop down menu item
            }
            else 
            {
                AppointmentsButton.IsEnabled = true; // drop down menu item
                PatientsButton.IsEnabled = true; // drop down menu item
                DentistButton.IsEnabled = false; // drop down menu item
                TreatmentButton.IsEnabled = false; // drop down menu item
            }
            

            if (!this.pressed)
            {
                //transactionsBtn.Background = Brushes.Gray; // change color to something else i guess - jedi
                this.pressed = true;
            } else
            {
                //transactionsBtn.Background = new SolidColorBrush(Color.FromRgb(189, 126, 74));
                this.pressed = false;
            }
        }

        private void Master_File_Click(object sender, RoutedEventArgs e)
        {
            if (DentistButton.IsEnabled && TreatmentButton.IsEnabled)
            {
                DentistButton.IsEnabled = false; // drop down menu item
                TreatmentButton.IsEnabled = false; // drop down menu item
            }
            else 
            {
                DentistButton.IsEnabled = true; // drop down menu item
                TreatmentButton.IsEnabled = true; // drop down menu item
                AppointmentsButton.IsEnabled = false; // drop down menu item
                PatientsButton.IsEnabled = false; // drop down menu item
            }
        }

        private void dashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new DashboardViewModel(DateTime.Today);
            AppointmentsButton.IsEnabled = false; // drop down menu item
            PatientsButton.IsEnabled = false; // drop down menu item
            //transactionsBtn.Background = new SolidColorBrush(Color.FromRgb(189, 126, 74));
        }

        private void Mainscreen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //make form draggable
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void PatientsButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PatientsViewModel();
        }

        private void AppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new AppointmentsViewModel();
        }

        private void userAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new UserAccountViewModel();
        }

        private void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new TreatmentViewModel();
        }

        private void DentistButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new DentistViewModel();
        }

        private void UtilityButton_CLick(object sender, RoutedEventArgs e)
        {
            DataContext = new UtilityViewModel();
        }

        //TODO CREATE A VIEW AND WINDOW MANAGER
    }
}
