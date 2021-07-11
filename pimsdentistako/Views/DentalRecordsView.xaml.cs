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
    /// Interaction logic for DentalRecordsView.xaml
    /// </summary>
    public partial class DentalRecordsView : Window
    {
        public DentalRecordsView()
        {
            InitializeComponent();
        }

        private void datagridDentalRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
