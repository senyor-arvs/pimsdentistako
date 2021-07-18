using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.DBElements
{
    public class EmergencyInfo
    {
        private string contactID;
        private string patientID;
        private string contactName;
        private string contactNumber;
        private string contactOfficeNumber;
        private string contactRelationship;
        private string contactOfficeNumberRemarks;

        public string ContactID { get => contactID; set => contactID = value; }
        public string PatientID { get => patientID; set => patientID = value; }
        public string ContactName { get => contactName; set => contactName = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
        public string ContactOfficeNumber { get => contactOfficeNumber; set => contactOfficeNumber = value; }
        public string ContactRelationship { get => contactRelationship; set => contactRelationship = value; }
        public string ContactOfficeNumberRemarks { get => contactOfficeNumberRemarks; set => contactOfficeNumberRemarks = value; }
    }
}
