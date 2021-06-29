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
            DataContext = new CustomCalendarViewModel();
        }

        private void showDateBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(CustomCalendar.myCalendar.SelectedDate.ToString());
        }

        private void onMouseLeave_CC(object sender, MouseEventArgs e)
        {
        }
    }
}
