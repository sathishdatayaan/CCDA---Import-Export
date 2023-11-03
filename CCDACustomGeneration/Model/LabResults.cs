using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class LabResultModel
    {
        public string code { get; set; } = "30954-2";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.3.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "Lab Results";
        public string title { get; set; } = "Lab Results";
    }
    public class LabResult
    {
        public string TestPerformed { get; set; } 
        public string TestResultn { get; set; }
        public string Units { get; set; } 
        public string NormalFindings { get; set; } 
        public string LonicCode { get; set; }
        public Nullable<DateTime> ReportDate { get; set; }
    }
}
