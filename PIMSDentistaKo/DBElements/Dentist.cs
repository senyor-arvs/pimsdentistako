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
                return BindFieldsByCondition(DatabaseHelper.BLANK_INPUT, DatabaseHelper.WHITE_SPACE_INPUT, 1, true, DentistFirstName, DentistMiddleName, DentistLastName, DentistSuffix);
            }
            set => dentistFullName = value;
        }

        //INTELLIGENT ALGORITHM TO BIND NAMES ALTHOUGH BLANK INPUTS EXIST
        public string BindFieldsByCondition(string condition_string, string string_to_add_if_match, int condition_mode, bool enableMiddleDot, params string[] strings_to_check)
        {
            int counter = 0;
            StringBuilder sb = new StringBuilder();
            foreach (string item in strings_to_check)
            {
                bool checker = item.Equals(condition_string);
                if (condition_mode != 0) //invert the condition if not in 0 mode
                {
                    checker = !checker;
                }
                if (checker)
                {
                    sb.Append(item);
                    if (counter == 1 && enableMiddleDot) sb.Append("."); //dot for middle initial
                    sb.Append(string_to_add_if_match);
                }
                counter++;
            }
            return sb.ToString().Trim(); ;
        }
    }


}
