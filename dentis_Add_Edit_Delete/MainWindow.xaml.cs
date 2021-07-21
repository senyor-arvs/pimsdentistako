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
using System.Data.OleDb;
using System.Collections.ObjectModel;

using dentis_Add_Edit_Delete.DBHelpers;
using dentis_Add_Edit_Delete.DBElements;

namespace dentis_Add_Edit_Delete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //int currentSelection;
        public MainWindow()
        {
            InitializeComponent();
            DatabaseHelper.Init();
            DentistHelper.MyDataGrid = dg; //attach your data grid to the helper so it can listen to it - Some Other Function may not work properly if you dont attach.
            DentistHelper.InitList(); 
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DentistHelper.ListenToDataGrid();
            DentistHelper.DisplaySelected(txtMainId, txtMainFirst, txtMainMiddle, txtMainLast, txtMainSuffix, txtMainLicense, txtMainPtr);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            addWindow addW = new addWindow();
            addW.Show();
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(!DentistHelper.IsCurrentlySelectedNull())
            {
                deleteWindow delW = new deleteWindow();
                delW.Show(); 
            }
           
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (!DentistHelper.IsCurrentlySelectedNull())
            {
                editWindow editW = new editWindow();
                editW.Show();
            }
        }

    }
}
