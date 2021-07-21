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
    /// Interaction logic for UserAccountView.xaml
    /// </summary>
    public partial class UserAccountView : UserControl
    {
        public UserAccountView()
        {
            InitializeComponent();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            saveButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Hidden;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            saveButton.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Visible;
        }
    }
}
