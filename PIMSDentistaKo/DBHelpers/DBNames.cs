using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.DBHelpers
{
    public class DBNames
    {
        public class ColumnNames
        {
            public static readonly string[] Dental_Record = { "dentalRecordID", "patientID", "treatmentPlanID", "dentalRecordTime", "dentalRecordDate", "dentalRecordToothNumber" };
            public static readonly string[] Dentist = { "dentistID", "dentistFirstName", "dentistMiddleName", "dentistLastName", "dentistSuffix", "dentistLicenseNumber", "dentistPTRNumber" };
            public static readonly string[] Emergency_Information = { "contactID", "patientID", "contactName", "contactNumber", "contactOfficeNumber", "contactRelationship", "contactOfficeNumberRemarks" };
            public static readonly string[] Patient = { "patientID", "patientFirstName", "patientMiddleName", "patientLastName", "patientSuffix", "patientNickname", "patientCivilStatus", "patientAddress", "patientEmail", "patientMobileNumber", "patientHomeNumber", "patientBirthdate", "patientSex", "patientReferredBy", "patientOccupation", "patientCompany", "patientOfficeNumber", "patientFaxNumber" };
            public static readonly string[] Treatment = { "treatmentID", "treatmentName" };
            public static readonly string[] Treatment_Plan = { "treatmentPlanID", "patientID", "treatmentID", "dentistID", "treatmentPlanTime", "treatmentPlanDate", "treatmentPlanGivenPrice", "treatmentPlanStatus" };
            public static readonly string[] Treatment_Type = { "treatmentTypeID", "treatmentTypeName", "treatmentID" };
            public static readonly string[] User_Account = { "accountID", "dentistID", "username", "password", "userAccountRemarks" };
        }

        public class TableNames
        {
            public static readonly string DENTAL_REC = "[Dental Record]";
            public static readonly string DENTIST = "[Dentist]";
            public static readonly string EMERGENCY_INFO = "[Emergency Information]";
            public static readonly string PATIENT = "[Patient]";
            public static readonly string TREATMENT = "[Treatment]";
            public static readonly string TREATMENT_PLAN = "[Treatment Plan]";
            public static readonly string TREATMENT_TYPE = "[Treatment Type]";
            public static readonly string USER_ACC = "[User Account]";
        }
        
    }
}
