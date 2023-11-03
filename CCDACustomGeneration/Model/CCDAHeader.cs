using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class CCDAHeader
    {
        public RealmCode realmCode { get; set; }
        public TypeId typeId { get; set; }
        public TemplateId TemplateId { get; set; }
        public Id Id { get; set; }
        public Code code { get; set; }
        public EffectiveTime effectiveTime { get; set; }
        public ConfidentialityCode confidentialityCode { get; set; }
        public LanguageCode languageCode { get; set; }
        public SetId setId { get; set; }
        public VersionNumber versionNumber { get; set; }
        public string title { get; set; }
    }

    public class RealmCode
    {
        public string code { get; set; } = "US";
    }
    public class TypeId
    {
        public string root { get; set; } = "2.16.840.1.113883.1.3";
        public string extension { get; set; } = "POCD_HD000040";
    }

    public class TemplateId
    {
        public string root { get; set; } = "2.16.840.1.113883.10.20.22.1.1";
    }
    public class Id
    {
        public string root { get; set; } = "2.16.840.1.113883.9.123";
        public string extension { get; set; } = "c266";
    }

    public class Code
    {
        public string code { get; set; } = "11488-4";
        public string codeSystem { get; set; } = "2.16.840.1.113883.6.1";
        public string codeSystemName { get; set; } = "LOINC";
        public string displayName { get; set; } = "Consultation note";
    }

    public class EffectiveTime
    {
        public DateTime value { get; set; } = DateTime.Now;
    }
    public class ConfidentialityCode
    {
        public string code { get; set; } = "N";
        public string codeSystem { get; set; } = "2.16.840.1.113883.5.25";
    }
    public class LanguageCode
    {
        public string code { get; set; } = "en-US";
    }
    public class SetId
    {
        public string root { get; set; } = "2.16.840.1.113883.19.7";
        public string extension { get; set; } = "BB35";
    }
    public class VersionNumber
    {
        public Int32 value { get; set; } = 1;
    }
}


