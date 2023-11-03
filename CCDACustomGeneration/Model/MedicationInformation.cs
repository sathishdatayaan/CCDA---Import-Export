using System;

namespace CreateClinicalReport.Model
{
    public class MedicationInformation
    {
        public string code { get; set; } = "10160-0";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.1.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "HISTORY OF MEDICATION USE";
        public string title { get; set; } = "MEDICATIONS";
    }

    public class PatientMedication
    {
        public Guid MedicationId { get; set; }
        public string Medication { get; set; }
        public string Strength { get; set; }
        public string Dosage { get; set; } 
        public string Dose { get; set; }
        public string doseUnit { get; set; } 
        public string Frequency { get; set; }
        public Boolean TakingCurrent { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string RxNorm { get; set; }
    }
}
