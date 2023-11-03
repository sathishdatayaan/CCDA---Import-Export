using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class ReasonForVisitModel 
    {
        public string code { get; set; } = "46239-0";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.13";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "REASON FOR VISIT + CHIEF COMPLAINT";
        public string title { get; set; } = "REASON FOR VISIT/CHIEF COMPLAINT";
    }

    public class ReasonForVisit
    {
        public string Description { get; set; } 
        public Nullable<DateTime> VisitDate { get; set; } 
    }
}
