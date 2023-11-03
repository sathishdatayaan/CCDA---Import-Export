using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class FunctionalStatusModel
    {
        public string code { get; set; } = "47420-5";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.14";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "FUNCTIONAL STATUS";
        public string title { get; set; } = "FUNCTIONAL STATUS";
    }

    public class FunctionalStatus
    {
        public string Code { get; set; } 
        public string Description { get; set; }
        public Nullable<DateTime> StatusDate { get; set; }
    }
}
