using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class PatientProblem 
    {
        public string code { get; set; } = "11450-4";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.5.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "PROBLEM LIST";
        public string title { get; set; } = "PROBLEMS";
    }

    public class PatientProblemes 
    {
        public Guid PatientGuid { get; set; }
        public int ProblemID { get; set; }
        public string ProblemCode { get; set; }
        public string Status { get; set; }
        public string DateDiagnosed { get; set; }
        public string Description { get; set; }
    }
}
