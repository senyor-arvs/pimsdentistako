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
    /// Interaction logic for UpdateAppointmentWindow.xaml
    /// </summary>
    public partial class UpdateAppointmentWindow : Window
    {
        bool performClosingCallBack = true;
        public IWindowCloseListener WindowCloseListener { get; set; }

        public UpdateAppointmentWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(performClosingCallBack) CallBackHelper.WindowCloseHelper.OnCloseConfig(this, WindowCloseListener);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallBackHelper.WindowCloseHelper.OnLoadedConfig(this, WindowCloseListener);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private void btnCloseUpdateAppointmentWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            performClosingCallBack = false;
            MoveTreatmentPlanWindow moveTreatmentPlanWindow = new MoveTreatmentPlanWindow();
            moveTreatmentPlanWindow.UpdateAppointmentWindow = this;
            moveTreatmentPlanWindow.WindowCloseListener = WindowCloseListener;
            moveTreatmentPlanWindow.Show();
            this.Hide();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            performClosingCallBack = true;
        }
    }
}
