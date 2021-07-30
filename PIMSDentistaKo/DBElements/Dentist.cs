using System;
using System.Collections.Generic;
using System.Text;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.DBElements
{
    public class Dentist
    {
        private string dentistID;
        private string dentistFirstName;
        private string dentistMiddleName;
        private string dentistLastName;
        private string dentistSuffix;
        private string dentistLicenseNumber;
        private string dentistPTRNumber;
        private string dentistFullName;

        public string DentistID { get => dentistID; set => dentistID = value; }
        public string DentistFirstName { get => dentistFirstName; set => dentistFirstName = value; }
        public string DentistMiddleName { get => dentistMiddleName; set => dentistMiddleName = value; }
        public string DentistLastName { get => dentistLastName; set => dentistLastName = value; }
        public string DentistSuffix { get => dentistSuffix; set => dentistSuffix = value; }
        public string DentistLicenseNumber { get => dentistLicenseNumber; set => dentistLicenseNumber = value; }
        public string DentistPTRNumber { get => dentistPTRNumber; set => dentistPTRNumber = value; }
        public string DentistFullName
        {
            get
            {
                return DatabaseHelper.BindFieldsByCondition(DatabaseHelper.BLANK_INPUT, DatabaseHelper.WHITE_SPACE_INPUT, 1, DentistMiddleName.Length <= 1, DentistFirstName, DentistMiddleName, DentistLastName, DentistSuffix);
            }
            set => dentistFullName = value;
        }

    }


}
