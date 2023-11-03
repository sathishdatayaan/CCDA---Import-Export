using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class ReasonForReferralModel 
    {
        public string code { get; set; } = "42349-1";
        public string root { get; set; } = "1.3.6.1.4.1.19376.1.5.3.1.3.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "REASON FOR REFERRAL";
        public string title { get; set; } = "REASON FOR REFERRAL";
    }
}
