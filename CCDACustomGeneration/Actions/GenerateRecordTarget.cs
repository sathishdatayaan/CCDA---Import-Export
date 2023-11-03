using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateRecordTarget 
    {
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;
        GenerateAddressPhNo addressphno;
        public string BindRecordTarget(string title, ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string recordtarget = string.Empty;
            var targetrole = clinicalDoc.RecordTarget.Append();
            var PRole = targetrole.PatientRole;
            hl7III = PRole.Id.Append();
            string SSN = string.Empty;
            if (patientinfo.ptDemographicDetail.SSN != "")
            { /// MANAGE Patient SSN
                SSN = patientinfo.ptDemographicDetail.SSN;
            }
            hl7III.Init("2.16.840.1.113883.19.5", SSN);/// Manage ID Tag UNDER Record Target
                                                       ///Patient Address Information                                           
            addressinfo = new AddressModel();
            addressinfo.street = patientinfo.ptDemographicDetail.Street;
            addressinfo.city = patientinfo.ptDemographicDetail.City;
            addressinfo.state = patientinfo.ptDemographicDetail.State;
            addressinfo.country = patientinfo.ptDemographicDetail.Country;
            addressinfo.pinCode = Convert.ToString(patientinfo.ptDemographicDetail.Zip);
            ///END
            ///Fill Address
            addressphno = new GenerateAddressPhNo();
            PRole.Addr.Add(addressphno.GenerateAddress(addressinfo, hl7factory));///FIll Patient Address
                                                                                 ///END
                                                                                 ///Patient Contact Information 
            contactinfo = new PhNoModel();
            contactinfo.telcomUse = "WP";
            contactinfo.telcomValue = patientinfo.ptDemographicDetail.ContactNo;
            contactinfo.nullFlavor = "UNK";
            PRole.Telecom.Add(addressphno.GeneratePhNo(contactinfo, hl7factory));///FIll Patient Contact Number  
                                                                                 ///END
            var PatientName = targetrole.PatientRole.Patient.Name.Append();///Manage Patient Name
            //Fill Patient Name
            nameinfo = new NameModel();
            nameinfo.Createengiven = patientinfo.ptDemographicDetail.FirstName;
            nameinfo.Createenfamily = patientinfo.ptDemographicDetail.LastName;
            addressphno.FillName(nameinfo, PatientName, hl7factory);///FIll Patient Name  
            //End
            var Patientbesic = targetrole.PatientRole.Patient;///Manage Patient Gender
            Patientbesic.AdministrativeGenderCode.CodeSystem = "2.16.840.1.113883.5.1";///FIll Patient Gender Value Constraint
            Patientbesic.AdministrativeGenderCode.Code = patientinfo.ptDemographicDetail.gender;//////FIll Patient Gender
            if (patientinfo.ptDemographicDetail.DateofBirth != "")///Manage Patient DOB
            {
                Patientbesic.BirthTime.Value = addressphno.GetDateWithFormat(patientinfo.ptDemographicDetail.DateofBirth);///Fill Patient DOB
            }
            if (patientinfo.ptDemographicDetail.Race != null && patientinfo.ptDemographicDetail.Race != "")///Manage Patient Race
            {
                string[] race = patientinfo.ptDemographicDetail.Race.Split(',');
                for (int i = 0; i < race.Length; i++)
                {
                    ICD icdObjects = hl7factory.CreateCD();
                    switch (race[i].ToString())
                    {
                        case "American Indian or Alaska Native":
                            Patientbesic.RaceCode.Code = "1002-5";
                            Patientbesic.RaceCode.DisplayName = "American Indian or Alaska Native";
                            Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        case "Asian":
                            Patientbesic.RaceCode.Code = "2028-9";
                            Patientbesic.RaceCode.DisplayName = "Asian";
                            Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        case "Black or African American":
                            Patientbesic.RaceCode.Code = "2054-5";
                            Patientbesic.RaceCode.DisplayName = "Black or African American";
                            Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        case "Hispanic or Latino":
                            Patientbesic.RaceCode.Code = "2135-2";
                            Patientbesic.RaceCode.DisplayName = "Hispanic or Latino";
                            Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        case "Native Hawaiian or Pacific Islander":
                            Patientbesic.RaceCode.Code = "2076-8";
                            Patientbesic.RaceCode.DisplayName = "Native Hawaiian or Pacific Islander";
                            Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        case "White":
                            Patientbesic.RaceCode.Code = "2106-3";
                            Patientbesic.RaceCode.DisplayName = "White";
                            Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        case "Some Other Race":
                            Patientbesic.RaceCode.Code = "2131-1";
                            Patientbesic.RaceCode.DisplayName = "White";
                            Patientbesic.RaceCode.CodeSystem = "Some Other Race";
                            Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                            break;
                        default:
                            Patientbesic.RaceCode.NullFlavor = "UNK";
                            break;

                    }
                }
            }
            if (patientinfo.ptDemographicDetail.Ethnicity != null && patientinfo.ptDemographicDetail.Ethnicity != "")///Manage Patient Ethnicity
            {
                switch (patientinfo.ptDemographicDetail.Ethnicity)
                {
                    case "Hispanic or Latino":
                        Patientbesic.EthnicGroupCode.Code = "2135-2";
                        Patientbesic.RaceCode.DisplayName = "Hispanic or Latino";
                        Patientbesic.RaceCode.CodeSystem = "2.16.840.1.113883.6.238";
                        Patientbesic.RaceCode.CodeSystemName = "Race & Ethnicity - CDC";
                        break;
                    case "Not Hispanic or Latino":
                        Patientbesic.EthnicGroupCode.Code = "2186-5";
                        Patientbesic.EthnicGroupCode.DisplayName = "Not Hispanic or Latino";
                        Patientbesic.EthnicGroupCode.CodeSystem = "2.16.840.1.113883.6.238";
                        Patientbesic.EthnicGroupCode.CodeSystemName = "Race & Ethnicity - CDC";
                        break;
                    default:
                        Patientbesic.EthnicGroupCode.NullFlavor = "UNK";
                        break;
                }
            }
            if (patientinfo.ptDemographicDetail.LanguageCode != null && patientinfo.ptDemographicDetail.LanguageCode != "")///Manage Patient LanguageCode
            {
                ILanguageCommunication langComm = hl7factory.CreateLanguageCommunication();
                langComm.LanguageCode.Code = patientinfo.ptDemographicDetail.LanguageCode;
                langComm.PreferenceInd.ValueSpecified = true;
                langComm.PreferenceInd.Value = true;
                Patientbesic.LanguageCommunication.Add(langComm);
            }
            return clinicalDoc.Xml;
        }

        protected void BindPatientName(PatientClinicalInformation patientinfo, IPN Name, Factory hl7factory)
        {
            if (patientinfo.ptDemographicDetail.FirstName != "")
            {
                var Given = hl7factory.Createengiven();
                Given = hl7factory.Createengiven();
                Given.Init(patientinfo.ptDemographicDetail.FirstName);
                Name.Items.Add(Given);
            }
            if (patientinfo.ptDemographicDetail.LastName != "")
            {
                var Family = hl7factory.Createenfamily();
                Family = hl7factory.Createenfamily();
                Family.Init(patientinfo.ptDemographicDetail.LastName);
                Name.Items.Add(Family);
            }

        }
    }
}
