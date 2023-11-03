using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class EncounterModel 
    {
        public string code { get; set; } = "46240-8";
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.2.22.1";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } 
        public string displayName { get; set; }
        public string title { get; set; } = "ENCOUNTERS";
    }
    public class Encounters 
    {
        public string EncounterDescription { get; set; }
        public string PerformerName { get; set; } 
        public string Location { get; set; }
        public string Code { get; set; } 
        public Nullable<DateTime> EncounterDate { get; set; }
    }

	public class EncountersFacility
	{
		public string EncounterNoteDate { get; set; }
		public string EncounterCode { get; set; }
		public string EncounterDescription { get; set; }
		public int facility_id { get; set; }
	}
}
