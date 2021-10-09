using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.Errors
{
    class ErrorCodes
    {
        public static int ERR_INV_PATIENT_SELECTION = 0x1;
        public static int ERR_INV_DENTIST_SELECTION = 0x2;
        public static int ERR_FAILED_ADDING_TREATMENT_TYPE = 0x3;
        public static int ERR_FAILED_ADDING_TREATMENT = 0x4;
        public static int ERR_UNSPECIFIED_TREATMENT = 0x5;
        public static int ERR_UNSPECIFIED_TREATMENT_TYPE = 0x6;
        public static int ERR_UNSPECIFIED_TREATMENT_EDITING = 0x7;
        public static int ERR_UNSPECIFIED_TREATMENT_TYPE_EDITING = 0x8;
        public static int ERR_INV_TREATMENT_SELECTION = 0x9;
        public static int ERR_INV_TREATMENT_TYPE_SELECTION = 0xA;
        public static int ERR_DELETING_DENTIST = 0xB;
        public static int ERR_DELETING_PATIENT = 0xC;
        public static int ERR_DELETING_TREATMENT = 0xD;
        public static int ERR_DELETING_TREATMENT_TYPE = 0xE;
    }
}
