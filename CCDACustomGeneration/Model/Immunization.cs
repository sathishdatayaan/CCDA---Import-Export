using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class ImmunizationModel 
    {
        public string code { get; set; } = "11369-6";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.2";
        public string root2 { get; set; } = "2.16.840.1.113883.10.20.22.2.2.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "History of immunizations";
        public string title { get; set; } = "Immunizations";
    }
    public class Immunization
    {
        public Nullable<DateTime> ApproximateDate { get; set; }
        public int CVX { get; set; } 
        public string Vaccine { get; set; } 
        public string Manufacturer { get; set; } 
    }
}
