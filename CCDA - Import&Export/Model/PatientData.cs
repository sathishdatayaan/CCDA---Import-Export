using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA___Import_Export.Model
{
	public class PatientData
	{
		public String GivenName { get; set; }
		public String FamilyName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public String Address { get; set; }
		public String Gender { get; set; }
		public String Id { get; set; }
		public String MothersId { get; set; }
		public String MothersName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public List<KeyValuePair<string   , string>> OtherIds { get; set; }
}
}
