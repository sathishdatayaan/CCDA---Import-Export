using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Model
{
    public class PatientDemographicDetail
    {
        public int ClientId { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string gender { get; set; }
        public string DateofBirth { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string ContactNo { get; set; }
        public string Race { get; set; }
        public string PreferredLanguage { get; set; }
        public string Ethnicity { get; set; }
        public string ReasonForReferral { get; set; }
        public string LanguageCode { get; set; }
    }
}
