using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.DBElements
{
    public class TreatmentPlan
    {
        private string treatmentPlanID;
        private string patientID;
        private string treatmentID;
        private string dentistID;
        private string treatmentPlanTime;
        private string treatmentPlanDate;
        private string treatmentPlanGivenPrice;
        private string treatmentPlanStatus;

        public string TreatmentPlanID { get => treatmentPlanID; set => treatmentPlanID = value; }
        public string PatientID { get => patientID; set => patientID = value; }
        public string TreatmentID { get => treatmentID; set => treatmentID = value; }
        public string DentistID { get => dentistID; set => dentistID = value; }
        public string TreatmentPlanTime { get => treatmentPlanTime; set => treatmentPlanTime = value; }
        public string TreatmentPlanDate { get => treatmentPlanDate; set => treatmentPlanDate = value; }
        public string TreatmentPlanGivenPrice { get => treatmentPlanGivenPrice; set => treatmentPlanGivenPrice = value; }
        public string TreatmentPlanStatus { get => treatmentPlanStatus; set => treatmentPlanStatus = value; }
    }
}
