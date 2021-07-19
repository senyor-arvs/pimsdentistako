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
using System.Data.OleDb;
using dentis_Add_Edit_Delete.ELclass;
using System.Collections.ObjectModel;

namespace dentis_Add_Edit_Delete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        //List<Dentist> dentistInfo;//
        private ObservableCollection<Dentist> dentistInfo;

        private readonly OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" +
            @"Data Source=|DataDirectory|\database\dentist_database.mdb;" +
            "User Id = Admin; Password=;");

        public ObservableCollection<Dentist> getDentistInfo ()
        {
            return this.dentistInfo;
        }

        public OleDbConnection getConnectionObject()
        {
            return this.con;
        }

        public MainWindow()
        {

            InitializeComponent();

            loadData();

        }

        public OleDbConnection GetConnection()
        {
            return this.con;
        }


        private void loadData()
        {
            //dentistInfo = new List<Dentist>();//
            dentistInfo = new ObservableCollection<Dentist>();
            dg.ItemsSource = dentistInfo;//nakadepende na ang laman ng datagrid sa dentist info na obesrvable collection//

            con.Open();
            OleDbCommand cmd = new OleDbCommand("select * from dentist_info", con);
            OleDbDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {

                Dentist dentist = new Dentist {
                    DentistId = dataReader["dentistId"].ToString(),
                    DentistFirstName = dataReader["dentistFirstName"].ToString(),
                    DentistMiddleName = dataReader["dentistMiddleName"].ToString(),
                    DentistLastName = dataReader["dentistLastName"].ToString(),
                    DentistSuffix = dataReader["dentistSuffix"].ToString(),
                    LicenseNumber = dataReader["licenseNumber"].ToString(),
                    PtrNumber = dataReader["ptrNumber"].ToString(),

                };


                /*
                Dentist dentist = new Dentist(
                dataReader["dentistId"].ToString(),
                dataReader["dentistFirstName"].ToString(),
                dataReader["dentistMiddleName"].ToString(),
                dataReader["dentistLastName"].ToString(),
                dataReader["dentistSuffix"].ToString(),
                dataReader["licenseNumber"].ToString(),
                dataReader["ptrNumber"].ToString()
                );
                */



                dentistInfo.Add(dentist);

                /*
                dg.Items.Add(new gridDentistItem { 
                    DENTITSTid = dentist.DentistId,
                    DENTISTfullname = dentist.DentistFirstName +" "+ dentist.DentistMiddleName + " " + dentist.DentistLastName,
                    DENTISTlicense = dentist.LicenseNumber,
                    DENTISTptr = dentist.PtrNumber
                });
                */

            }
            con.Close();
        }


        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            addWindow addW = new addWindow(this);
            //nilagyan ng this ang loob ng addwindow//

            addW.Show();
        }


        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteWindow delW = new deleteWindow();
            delW.Show();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            
            editWindow editW = new editWindow();
            editW.Show();
            

            /*
            Dentist dentist = new Dentist
            {
                DentistId = 69.ToString(),
                DentistFirstName = "arwin",
                DentistMiddleName = "santos",
                DentistLastName = "delacruz",
                DentistSuffix = "malupet",
                LicenseNumber = 12341.ToString(),
                PtrNumber = 2345234.ToString()

            };


            dentistInfo.Add(dentist);
            */

            //adding in the datagrid//

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dg.SelectedIndex;
            Dentist dentist = dentistInfo[index];


            txtMainId.Text = dentist.DentistId;
            txtMainFirst.Text = dentist.DentistFirstName;
            txtMainMiddle.Text = dentist.DentistMiddleName;
            txtMainLast.Text = dentist.DentistLastName;
            txtMainSuffix.Text = dentist.DentistSuffix;
            txtMainLicense.Text = dentist.LicenseNumber;
            txtMainPtr.Text = dentist.PtrNumber;

        }

        

       
    }
}
