using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Model
{
    public class AllergyTable
    {
        public string substance { get; set; }
        public string reaction { get; set; }
        public string rxNorm { get; set; }
        public string status { get; set; }
        public string allergyDate { get; set; }
    }
}
