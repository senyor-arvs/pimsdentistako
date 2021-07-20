using pimsdentistako.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
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
using static pimsdentistako.DBHelpers.TreatmentHelper;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for TreatmentView.xaml
    /// </summary>
    public partial class TreatmentView : UserControl
    {
        public TreatmentView()
        {
            InitializeComponent();
            InitList();
            treatmentDataGrid.ItemsSource = TreatmentList;
        }

        private void addTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            _ = AddTreatment(treatmentTxtBox.Text.Trim());
        }
    }
}
