using System;
using System.Collections.Generic;
using System.Text;

namespace dentis_Add_Edit_Delete.DBElements
{
    public class UserAccount
    {
        private string dentistID;
        private string username;
        private string password;
        private string userAccountRemarks;

        public string DentistID { get => dentistID; set => dentistID = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string UserAccountRemarks { get => userAccountRemarks; set => userAccountRemarks = value; }
    }
}
