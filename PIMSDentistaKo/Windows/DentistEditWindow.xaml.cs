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
    /// Interaction logic for DentistEditWindow.xaml
    /// </summary>
    public partial class DentistEditWindow : Window
    {
        public DentistEditWindow()
        {
            InitializeComponent();
        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            
        }
    }
}
