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
    /// Interaction logic for AddEditTreatmentPlan.xaml
    /// </summary>
    public partial class AddEditTreatmentPlanWindow : Window
    {
        public IWindowCloseListener WindowCloseListener { get; set; }

        public AddEditTreatmentPlanWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
