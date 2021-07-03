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
            DataContext = new DashboardViewModel();
            Mainscreen.Height = 1080 * 0.95;
            Mainscreen.Width = 1920 * 0.95;
        }

        private void ProfilePicture_LeftClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hello!");
        }

        private void transactionsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentsButton.IsEnabled == true && PatientsButton.IsEnabled == true)
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
                DataContext = new TransactionViewModel();
                transactionsBtn.Background = Brushes.Gray; // change color to something else i guess - jedi
                this.pressed = true;
            } else
            {
                DataContext = new DashboardViewModel();
                transactionsBtn.Background = new SolidColorBrush(Color.FromRgb(189, 126, 74));
                this.pressed = false;
            }
        }

        private void Master_File_Click(object sender, RoutedEventArgs e)
        {
            if (DentistButton.IsEnabled == true && TreatmentButton.IsEnabled == true)
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
    }
}
