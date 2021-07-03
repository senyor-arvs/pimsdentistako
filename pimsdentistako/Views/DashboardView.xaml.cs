using pimsdentistako.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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


    }
}
