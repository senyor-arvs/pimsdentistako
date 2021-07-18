using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.DBElement
{
    public class TreatmentType
    {
        private string treatmentTypeID;
        private string treatmentTypeName;
        private string treatmentID;

        public string TreatmentTypeID { get => treatmentTypeID; set => treatmentTypeID = value; }
        public string TreatmentTypeName { get => treatmentTypeName; set => treatmentTypeName = value; }
        public string TreatmentID { get => treatmentID; set => treatmentID = value; }
    }
}
