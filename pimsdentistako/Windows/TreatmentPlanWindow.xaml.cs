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

namespace pimsdentistako.Windows
{
    /// <summary>
    /// Interaction logic for TreatmentPlanWindow.xaml
    /// </summary>
    public partial class TreatmentPlanWindow : Window
    {
        private string patientName;
        private string patientNumber;
        public TreatmentPlanWindow(string patientName, string patientNumber)
        {
            this.patientName = patientName;
            this.patientNumber = patientNumber;
            InitializeComponent();
        }
    }
}
