using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using pimsdentistako.DBHelpers;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            // Initializing text components and timer
            timeTxt.Content = DateTime.Now.ToShortTimeString();
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += timer_Tick;
            timer.Start();

            DatabaseHelper.Init();
            PatientHelper.InitList();
            string totalPatient = "0";
            int total = PatientHelper.PatientList.Count;
            if (total >= 19999)
            {
                totalPatient = "20000+";
            } else
            {
                totalPatient = total.ToString();
            }
            txtBlockPatientsTotal.Text = totalPatient;
        }

        private void showDateBtn_Click(object sender, RoutedEventArgs e)
        {
            ///MessageBox.Show(CustomCalendar.myCalendar.SelectedDate.ToString());
        }

        private void onMouseLeave_CC(object sender, MouseEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void customCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timeTxt.Content = DateTime.Now.ToShortTimeString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
            
        }
      

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Minimized;
            }

        }
    }
}
