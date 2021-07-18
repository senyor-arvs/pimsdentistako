using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.DBElement
{
    public class Treatment
    {
        private string treatmentID;
        private string treatmentName;

        public string TreatmentID { get => treatmentID; set => treatmentID = value; }
        public string TreatmentName { get => treatmentName; set => treatmentName = value; }
    }
}
