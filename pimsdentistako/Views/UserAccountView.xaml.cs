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

using pimsdentistako.DBHelpers;
using pimsdentistako.DBElements;

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
            UserAccountHelper.InitList();
            administratorName.Text = UserAccountHelper.GetAdminFullName();
            usernameTxtBox.Text = administratorName.Text;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            saveButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Hidden;
            setTxtBoxEnable(false);
        }
        

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserAccountHelper.UpdatePassword(oldPasswordTxtBox, newPasswordTxtBox, retypePasswordTxtBox))
            {
                DatabaseHelper.DisplayDialog("Password Changed", "Your password was changed.");
                saveButton.Visibility = Visibility.Hidden;
                updateButton.Visibility = Visibility.Visible;
                setTxtBoxEnable(true);
                setTextBoxTextCleared();
            }
        }

        private void setTxtBoxEnable(bool enable)
        {
            oldPasswordTxtBox.IsReadOnly = enable;
            newPasswordTxtBox.IsReadOnly = enable;
            retypePasswordTxtBox.IsReadOnly = enable;
        }

        private void setTextBoxTextCleared()
        {
            oldPasswordTxtBox.Clear();
            newPasswordTxtBox.Clear();
            retypePasswordTxtBox.Clear();
        }
    }
}
