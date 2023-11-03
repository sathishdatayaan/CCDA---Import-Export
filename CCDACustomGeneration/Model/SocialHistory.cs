using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class SocialHistory 
    {
        public string code { get; set; } = "29762-2";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.17";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "Social History";
        public string title { get; set; } = "Social History";
    }

    public class SocialHistoryModel 
    {
        public string Smoker { get; set; }
        public string EntryDate { get; set; }
        public string Alcohol { get; set; }
        public string Drugs { get; set; }
        public string Tobacoo { get; set; }
    }
}
