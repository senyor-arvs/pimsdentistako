using System.Windows;
using System.Windows.Controls;
using pimsdentistako.Windows;

using pimsdentistako.DBHelpers;
using pimsdentistako.Callbacks;

namespace pimsdentistako.Views
{
    /// <summary>
    /// Interaction logic for DentistView.xaml
    /// </summary>
    public partial class DentistView : UserControl, IWindowCloseListener
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
            addWindow.WindowCloseListener = this;
            addWindow.Show();
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!DentistHelper.IsCurrentlySelectedNull())
            {
                DentistEditWindow editW = new DentistEditWindow();
                editW.WindowCloseListener = this;
                editW.Show();
            } 
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_DENTIST_SELECTION);
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!DentistHelper.IsCurrentlySelectedNull())
            {
                DeleteConfimationWindow delW = new DeleteConfimationWindow(
                    DeleteConfimationWindow.DeleteWindowModes.DENTIST_MODE);
                delW.WindowCloseListener = this;
                delW.Show();
            }
            else
            {
                DatabaseHelper.DisplayWarningDialog(Errors.ErrorCodes.ERR_INV_DENTIST_SELECTION);
            }
        }

        public void OnWindowClose(bool isClosed)
        {
            Window.GetWindow(this).IsEnabled = isClosed;
        }
    }
}
