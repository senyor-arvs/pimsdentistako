using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.DBElements
{
    public class DentalRecord
    {
        private string dentalRecordID;
        private string patientID;
        private string treatmentPlanID;
        private string dentalRecordTime;
        private string dentalRecordDate;
        private string dentalRecordToothNumber;

        public string DentalRecordID { get => dentalRecordID; set => dentalRecordID = value; }
        public string PatientID { get => patientID; set => patientID = value; }
        public string TreatmentPlanID { get => treatmentPlanID; set => treatmentPlanID = value; }
        public string DentalRecordTime { get => dentalRecordTime; set => dentalRecordTime = value; }
        public string DentalRecordDate { get => dentalRecordDate; set => dentalRecordDate = value; }
        public string DentalRecordToothNumber { get => dentalRecordToothNumber; set => dentalRecordToothNumber = value; }
    }
}
