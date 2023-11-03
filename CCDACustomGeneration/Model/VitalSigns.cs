using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class VitalSigns 
    {
        public Guid VitalsID { get; set; }
        public Nullable<DateTime> Entrydate { get; set; }
        public string BloodPressure { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> WEIGHT { get; set; }
        public string BloodPressureSystolic { get;  set; }
        public string BloodPressureDiastolic { get;  set; }
        public DateTime? VitalDate { get; internal set; }
        public string WeightUnit { get;  set; }
        public string HeightUnit { get;  set; }
    }

    public class VitalSignsCode 
    {
        public string code { get; set; } = "8716-3";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.4.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "VITAL SIGNS";
        public string title { get; set; } = "VITAL SIGNS";
    }
}
