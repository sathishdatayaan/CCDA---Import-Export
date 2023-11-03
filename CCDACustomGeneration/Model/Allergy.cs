using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class Allergy
    {
        public string code { get; set; }= "48765-2";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.6.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; }= "LOINC";
        public string displayName { get; set; } = "ALLERGIES";
        public string title { get; set; }= "ALLERGIES, ADVERSE REACTIONS, ALERTS";
    }

    public class PatientAllergies
    {
        public int allergyId { get; set; }
        public string substance { get; set; }
        public string reaction { get; set; } 
        public string rxNorm { get; set; } 
        public string status { get; set; } 
        public string allergyDate { get; set; } 
    }
}
