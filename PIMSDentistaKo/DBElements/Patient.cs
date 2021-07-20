using System;
using System.Collections.Generic;
using System.Text;
using pimsdentistako.DBHelpers;

namespace pimsdentistako.DBElements
{
    public class Patient
    {
        private string patientID;
        private string patientFirstName;
        private string patientMiddleName;
        private string patientLastName;
        private string patientSuffix;
        private string patientNickname;
        private string patientCivilStatus;
        private string patientAddress;
        private string patientEmail;
        private string patientMobileNumber;
        private string patientHomeNumber;
        private string patientBirthdate;
        private string patientSex;
        private string patientReferredBy;
        private string patientOccupation;
        private string patientCompany;
        private string patientOfficeNumber;
        private string patientFaxNumber;

        public string PatientID { get => patientID; set => patientID = value; }
        public string PatientFirstName { get => patientFirstName; set => patientFirstName = value; }
        public string PatientMiddleName { get => patientMiddleName; set => patientMiddleName = value; }
        public string PatientLastName { get => patientLastName; set => patientLastName = value; }
        public string PatientSuffix { get => patientSuffix; set => patientSuffix = value; }
        public string PatientNickname { get => patientNickname; set => patientNickname = value; }
        public string PatientCivilStatus { get => patientCivilStatus; set => patientCivilStatus = value; }
        public string PatientAddress { get => patientAddress; set => patientAddress = value; }
        public string PatientEmail { get => patientEmail; set => patientEmail = value; }
        public string PatientMobileNumber { get => patientMobileNumber; set => patientMobileNumber = value; }
        public string PatientHomeNumber { get => patientHomeNumber; set => patientHomeNumber = value; }
        public string PatientBirthdate { get => patientBirthdate; set => patientBirthdate = value; }
        public string PatientSex { get => patientSex; set => patientSex = value; }
        public string PatientReferredBy { get => patientReferredBy; set => patientReferredBy = value; }
        public string PatientOccupation { get => patientOccupation; set => patientOccupation = value; }
        public string PatientCompany { get => patientCompany; set => patientCompany = value; }
        public string PatientOfficeNumber { get => patientOfficeNumber; set => patientOfficeNumber = value; }
        public string PatientFaxNumber { get => patientFaxNumber; set => patientFaxNumber = value; }

        public static string GetAgeByBirth(string date_of_birth)
        {
            DateTime dateOfBirth = DatabaseHelper.ConvertToDateTime(date_of_birth);
            DateTime today = DateTime.Now;
            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth >= today.AddYears(-age)) age--;

            return age.ToString();
        }
    }
}
