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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using dentis_Add_Edit_Delete.ELclass;
using System.Collections.ObjectModel;

namespace dentis_Add_Edit_Delete
{
    /// <summary>
    /// Interaction logic for addWindow.xaml
    /// </summary>
    public partial class addWindow : Window
    {
        // private MainWindow main;//
        private MainWindow parentInstance;
        ObservableCollection<Dentist> dentistList;
        OleDbConnection con;

        //private readonly OleDbConnection connection;//


        /*OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" +
            @"Data Source=|DataDirectory|\database\dentist_database.mdb;" +
            "User Id = Admin; Password=;");*/

        public addWindow(MainWindow parentInstance)
        {
            InitializeComponent();

            this.parentInstance = parentInstance;
            con = this.parentInstance.getConnectionObject();
            this.dentistList = this.parentInstance.getDentistInfo();
            //main = instance;//
            //this.connection = instance.GetConnection();//
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            con.Open();

            OleDbCommand getMaxId = new OleDbCommand("select MAX(dentistId) from [dentist_info]",con);
            Int32 maxAvailable = (Int32)getMaxId.ExecuteScalar();
            maxAvailable += 1;

            
            
            Dentist dentist = new Dentist
            {
                DentistId = maxAvailable.ToString(),
                DentistFirstName = txtAddFirst.Text,
                DentistMiddleName = txtAddMiddle.Text,
                DentistLastName = txtAddLast.Text,
                DentistSuffix = txtAddSuffix.Text,
                LicenseNumber = txtAddLicense.Text,
                PtrNumber = txtAddPtr.Text

            };
             
            OleDbCommand insertcommand = new OleDbCommand(
                @"INSERT INTO [dentist_info] (dentistFirstName,dentistMiddleName,dentistLastName,dentistSuffix,LicenseNumber,ptrNumber) VALUES (@fname,@mname,@lname,@suffix,@license,@ptr)",con);

            insertcommand.Parameters.Add(new OleDbParameter("@fname", dentist.DentistFirstName));
            insertcommand.Parameters.Add(new OleDbParameter("@mname", dentist.DentistMiddleName));
            insertcommand.Parameters.Add(new OleDbParameter("@lname", dentist.DentistLastName));
            insertcommand.Parameters.Add(new OleDbParameter("@suffix", dentist.DentistSuffix));
            insertcommand.Parameters.Add(new OleDbParameter("@license", dentist.LicenseNumber));
            insertcommand.Parameters.Add(new OleDbParameter("@ptr", dentist.PtrNumber));

            insertcommand.ExecuteNonQuery();
            con.Close();

            this.dentistList.Add(dentist);

            MainWindow m = new MainWindow();
            MessageBox.Show("Data is saved.");
            this.Close();
            











            /*
            connection.Open();
            OleDbCommand maxCommand = new OleDbCommand("select max(dentistId) from dentist_info", connection);
            Int32 maxAvailable = (Int32)maxCommand.ExecuteScalar();
            maxAvailable += 1;

            Dentist dentist = new Dentist
            

            {
                DentistId = maxAvailable.ToString(),
                DentistFirstName = txtAddFirst.Text,
                DentistMiddleName = txtAddMiddle.Text,
                DentistLastName = txtAddLicense.Text,
                DentistSuffix = txtAddSuffix.Text,
                LicenseNumber = txtAddLicense.Text,
                PtrNumber = txtAddPtr.Text

            };

            main.dentistInfo.Add(dentist);
            */

        }


        
    }
}
