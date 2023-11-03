using CreateClinicalReport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class PatientRecordTarget
    {
        public PRecordTarget recordTarget { get; set; }
    }

    public class PRecordTarget
    {
        public string typeCode { get; set; }
        public string contextControlCode { get; set; }
        public PatientTargetRole patientRole { get; set; }
    }
    public class PatientTargetRole
    {
        public string classCode { get; set; }
        public Id id { get; set; }
        public Address address { get; set; }
        public TargetTelecom telecom { get; set; }
        public TargetPatient patient { get; set; }
    }

    public class Address
    {
        public string streetAddressLine { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
    }
    public class TargetTelecom
    {
        public string value { get; set; }
        public string use { get; set; }
    }

    public class TargetPatient
    {
        public string classCode { get; set; }
        public string determinerCode { get; set; }
        public PatientName name { get; set; }
        public PatientAdministrativeGenderCode administrativeGenderCode { get; set; }
        public BirthTime birthTime { get; set; }
        public PatientRaceCode raceCode { get; set; }
        public PatientEthnicGroupCode ethnicGroupCode { get; set; }
        public PatientLanguageCommunication languageCommunication { get; set; }
    }
    public class BirthTime
    {
        public string value { get; set; }
    }
    public class PatientName
    {
        public string given { get; set; }
        public string family { get; set; }
    }
    public class PatientAdministrativeGenderCode
    {
        public string code { get; set; }
        public string codeSystem { get; set; }
    }
    public class PatientRaceCode
    {
        public string code { get; set; }
        public string codeSystem { get; set; }
        public string codeSystemName { get; set; }
        public string DisplayName { get; set; }
    }
    public class PatientEthnicGroupCode
    {
        public string code { get; set; }
        public string codeSystem { get; set; }
        public string codeSystemName { get; set; }
        public string DisplayName { get; set; }
    }

    public class PatientLanguageCommunication
    {
        public LanguageCode languageCode { get; set; }
        public PatientPreferenceInd preferenceInd { get; set; }
    }

    public class PatientPreferenceInd
    {
        public string valueSpecified { get; set; }
        public string Value { get; set; }
    }
}
