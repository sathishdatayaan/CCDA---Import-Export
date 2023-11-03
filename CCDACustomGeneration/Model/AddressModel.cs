using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Model
{
    public class AddressModel
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pinCode { get; set; }
        public string nullFlavor { get; set; } = "UNK";

    }
}
