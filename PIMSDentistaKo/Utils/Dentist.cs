using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.Utils {

    class Dentist
    {
        private String dentistFirstName;
        private String dentistMiddleName;
        private String dentistLastName;
        private String dentistLicenseNumber;
        private String dentistPTR;

        public Dentist(string dentistFirstName, string dentistMiddleName, string dentistLastName, string dentistLicenseNumber, string dentistPTR)
        {
            this.dentistFirstName = dentistFirstName;
            this.dentistMiddleName = dentistMiddleName;
            this.dentistLastName = dentistLastName;
            this.dentistLicenseNumber = dentistLicenseNumber;
            this.dentistPTR = dentistPTR;
        }

        public String getFirstName()
        {
            return this.dentistFirstName;
        }

        public String getMiddleName()
        {
            return this.dentistMiddleName;
        }

        public String getLastName()
        {
            return this.dentistLastName;
        }

        public String getFullName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.dentistFirstName).Append(" ").Append(this.dentistMiddleName).Append(" ").Append(this.dentistLastName);
            return sb.ToString();
        }

        public String getLicNumber()
        {
            return this.dentistLicenseNumber;
        }

        public String getPTRNumber()
        {
            return this.dentistPTR;
        }

    }
}
