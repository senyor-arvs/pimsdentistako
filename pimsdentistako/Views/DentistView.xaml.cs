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
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void New_btn_Click(object sender, RoutedEventArgs e)
        {
            DentistAddWindow addWindow = new DentistAddWindow();
            addWindow.Show();
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            DentistEditWindow editWindow = new DentistEditWindow();
            editWindow.Show();

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            DentistDeleteWindow deleteWindow = new DentistDeleteWindow();
            deleteWindow.Show();
        }
    }
}
