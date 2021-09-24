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
using System.Windows.Shapes;
using pimsdentistako.Callbacks;

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for TreatmentPlanWindow.xaml
    /// </summary>
    public partial class TreatmentPlanWindow : Window
    {
        private IWindowCloseListener windowCloseListener;   
        private string patientName;
        private string patientNumber;

        public IWindowCloseListener WindowCloseListener { get => windowCloseListener; set => windowCloseListener = value; }

        public TreatmentPlanWindow(string patientName, string patientNumber)
        {
            this.patientName = patientName;
            this.patientNumber = patientNumber;
            InitializeComponent();
        }

        private void FitToContent()
        {
            // where dg is my data grid's name...
            foreach (DataGridColumn column in treatmentPlanDataGrid.Columns)
            {
                //if you want to size your column as per the cell content
                //column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells);
                //if you want to size your column as per the column header
                //column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                //if you want to size your column as per both header and cell content
                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FitToContent();
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private void btnCloseTreatmentPlanWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
