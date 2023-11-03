using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA___Import_Export.Model
{
	public class PhysicianData
	{
		public string Name { get; set; }
		public string AddressLine { get; set; }
		public string City { get; set; }
		public string Postal { get; set; }
		public string Id { get; set; }
		public string PhysicianId { get; set; }
		public string[] PhysicianName { get; set; }
		public string OrgId { get; set; }
		public string OrgName { get; set; }
	}
}
