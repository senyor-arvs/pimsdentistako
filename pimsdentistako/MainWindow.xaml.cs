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
using pimsdentistako.ViewModels;

namespace pimsdentistako
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool pressed = false;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }

        private void ProfilePicture_LeftClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hello!");
        }

        private void transactionsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.pressed)
            {
                DataContext = new TransactionViewModel();
                transactionsBtn.Background = Brushes.Gray;
                this.pressed = true;
            } else
            {
                DataContext = new DashboardViewModel();
                transactionsBtn.Background = new SolidColorBrush(Color.FromRgb(189, 126, 74));
                this.pressed = false;
            }
            
        }
    }
}
