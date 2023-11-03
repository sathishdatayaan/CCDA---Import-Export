using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class DocumentationOfList 
    {
        public int staffId { get; set; }
        public string date { get; set; }
        public string staffName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pinCode { get; set; }
    }
}
