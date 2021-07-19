using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dentis_Add_Edit_Delete.ELclass
{
    public class Dentist
    {
        private string dentistId;
        private string dentistFirstName;
        private string dentistMiddleName;
        private string dentistLastName;
        private string dentistSuffix;
        private string licenseNumber;
        private string ptrNumber;

        public string DentistId { get => dentistId; set => dentistId = value; }
        public string DentistFirstName { get => dentistFirstName; set => dentistFirstName = value; }
        public string DentistMiddleName { get => dentistMiddleName; set => dentistMiddleName = value; }
        public string DentistLastName { get => dentistLastName; set => dentistLastName = value; }
        public string DentistSuffix { get => dentistSuffix; set => dentistSuffix = value; }
        public string LicenseNumber { get => licenseNumber; set => licenseNumber = value; }
        public string PtrNumber { get => ptrNumber; set => ptrNumber = value; }


        /*
       
        public Dentist(
            string dentistId,
            string dentistFirstName,
            string dentistMiddleName,
            string dentistLastName,
            string dentistSuffix,
            string licenseNumber,
            string ptrNumber)
        {
            this.dentistId = dentistId;
            this.DentistFirstName = dentistFirstName;
            this.DentistMiddleName = dentistMiddleName;
            this.DentistLastName = dentistLastName;
            this.DentistSuffix = dentistSuffix;
            this.LicenseNumber = licenseNumber;
            this.PtrNumber = ptrNumber;
        }
    */

        /*
       
        public string DentistId { get => this.dentistId; set => this.dentistId = value; }
        public string DentistFirstName { get => dentistFirstName; set => dentistFirstName = value; }
        public string DentistMiddleName { get => dentistMiddleName; set => dentistMiddleName = value; }
        public string DentistLastName { get => dentistLastName; set => dentistLastName = value; }
        public string DentistSuffix { get => dentistSuffix; set => dentistSuffix = value; }
        public string LicenseNumber { get => licenseNumber; set => licenseNumber = value; }
        public string PtrNumber { get => ptrNumber; set => ptrNumber = value; }
        */

    }
        
}
