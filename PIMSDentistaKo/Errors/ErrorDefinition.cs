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
            {ErrorCodes.ERR_INV_DENTIST_SELECTION, new Tuple<string, string>("Invalid Selection", "No dentist is selected.\nPlease select a dentist from the list.")},
            {ErrorCodes.ERR_FAILED_ADDING_TREATMENT, new Tuple<string, string>("Adding Treatment Failed", "No Treatment was added.\nAn error occured while adding Treatment.")},
            {ErrorCodes.ERR_FAILED_ADDING_TREATMENT_TYPE, new Tuple<string, string>("Adding Treatment Type Failed", "No Treatment Type was added.\nAn error occured while adding Treatment Type for the current Treatment.")},
            {ErrorCodes.ERR_UNSPECIFIED_TREATMENT, new Tuple<string, string>("Treatment Adding", "Please specify the name of the Treatment that you want to add.")},
            {ErrorCodes.ERR_UNSPECIFIED_TREATMENT_TYPE, new Tuple<string, string>("Treatment Type Adding", "Please specify the name of the Treatment Type that you want to add.")},
            {ErrorCodes.ERR_UNSPECIFIED_TREATMENT_EDITING, new Tuple<string, string>("Treatment Editing", "Please specify the new name of the Treatment.")},
            {ErrorCodes.ERR_UNSPECIFIED_TREATMENT_TYPE_EDITING, new Tuple<string, string>("Treatment Type Editing", "Please specify the new name of the Treatment Type.")},
            {ErrorCodes.ERR_INV_TREATMENT_SELECTION, new Tuple<string, string>("Invalid Selection", "No Treatment is selected.\nPlease select a Treatment from the list.")},
            {ErrorCodes.ERR_INV_TREATMENT_TYPE_SELECTION, new Tuple<string, string>("Invalid Selection", "No Treatment Type is selected.\nPlease select a Treatment Type from the list.")},
            {ErrorCodes.ERR_DELETING_DENTIST, new Tuple<string, string>("Deleting Dentist", "An error occured while deleting Dentist.\nNothing was deleted")},
            {ErrorCodes.ERR_DELETING_PATIENT, new Tuple<string, string>("Deleting Patient", "An error occured while deleting Patient.\nNothing was deleted")},
            {ErrorCodes.ERR_DELETING_TREATMENT, new Tuple<string, string>("Deleting Treatment", "An error occured while deleting Treatment.\nNothing was deleted")},
            {ErrorCodes.ERR_DELETING_TREATMENT_TYPE, new Tuple<string, string>("Deleting Treatment Type", "An error occured while deleting Treatment Type.\nNothing was deleted")}


    };
    }
}
