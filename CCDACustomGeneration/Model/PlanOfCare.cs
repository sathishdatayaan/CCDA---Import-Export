using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class PlanOfCareModel
    {
        public string code { get; set; } = "18776-5";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.10";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "Treatment plan";
        public string title { get; set; } = "Care Plans";
    }
    public class PlanOfCare 
    {
        public string Goal { get; set; }
        public string Instructions { get; set; }
        public Nullable<DateTime> PlannedDate { get; set; } 
    }
}
