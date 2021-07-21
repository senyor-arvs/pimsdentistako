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
using pimsdentistako.Windows;

using pimsdentistako.DBElements;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for DentistView.xaml
    /// </summary>
    public partial class DentistView : UserControl
    {
        public DentistView()
        {
            InitializeComponent();
            DentistHelper.MyDataGrid = dentistDataGrid;
            DentistHelper.InitList();
            
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DentistHelper.ListenToDataGrid();
            DentistHelper.DisplaySelected(txtFirst, txtMiddle, txtLast, txtSuffix, txtLicense, txtPtr);
        }

        private void New_btn_Click(object sender, RoutedEventArgs e)
        {
            DentistAddWindow addWindow = new DentistAddWindow();
            addWindow.Show();
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!DentistHelper.IsCurrentlySelectedNull())
            {
                DentistEditWindow editW = new DentistEditWindow();
                editW.Show();
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!DentistHelper.IsCurrentlySelectedNull())
            {
                DentistDeleteWindow delW = new DentistDeleteWindow();
                delW.Show();
            }
        }
    }
}
