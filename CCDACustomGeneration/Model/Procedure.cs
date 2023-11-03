using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class ProcedureModel 
    {
        public string code { get; set; } = "47519-4";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.7";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "PROCEDURES";
        public string title { get; set; } = "PROCEDURES";
    }

    public class ProcedureList 
    {
        public string Description { get; set; }
        public string CPTCodes { get; set; } 
    }
}
