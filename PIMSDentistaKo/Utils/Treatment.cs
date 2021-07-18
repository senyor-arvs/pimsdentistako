using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.Utils
{
    public class Treatment
    {
        private string treatmentID;
        private string treatmentName;

        public Treatment(string treatmentID, string treatmentName)
        {
            this.treatmentID = treatmentID;
            this.treatmentName = treatmentName;
        }
        public string TreatmentID { get => treatmentID; set => treatmentID = value; }
        public string TreatmentName { get => treatmentName; set => treatmentName = value; }
    }
}
