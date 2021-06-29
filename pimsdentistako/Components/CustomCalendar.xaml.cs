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

namespace pimsdentistako.Components
{
    /// <summary>
    /// Interaction logic for CustomCalendar.xaml
    /// </summary>
    public partial class CustomCalendar : UserControl
    {
        public CustomCalendar()
        {
            InitializeComponent();
        }

        private void CustomCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null); 
        }

      








    }
}
