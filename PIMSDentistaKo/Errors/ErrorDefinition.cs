using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.Errors
{
    class ErrorDefinition
    {
        //TODO IMPLEMENT OTHER ERROR CODES
        public static readonly IDictionary<int, Tuple<string, string>> Definitions = new Dictionary<int, Tuple<string, string>>()
        {
            {ErrorCodes.ERR_INV_PATIENT_SELECTION, new Tuple<string, string>("Invalid Selection", "No patient is selected.\nPlease select a patient from the list.")},
            {ErrorCodes.ERR_INV_DENTIST_SELECTION, new Tuple<string, string>("Invalid Selection", "No dentist is selected.\nPlease select a dentist from the list.")}
        };

    }
}
