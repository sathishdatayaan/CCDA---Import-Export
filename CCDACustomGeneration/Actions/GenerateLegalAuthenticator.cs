using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateLegalAuthenticator
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;
        public string FillLegalAuthenticatorInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string legalAuthenticatordetais = string.Empty;
            var assignedEntity = clinicalDoc.LegalAuthenticator.AssignedEntity;
            clinicalDoc.LegalAuthenticator.Time.Init(DateTime.Now);
            clinicalDoc.LegalAuthenticator.SignatureCode.Init("S");
            hl7III = assignedEntity.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6", "KP00017");
            assignedEntity.Code.Code = "207QA0505X";
            assignedEntity.Code.CodeSystem = "2.16.840.1.113883.6.101";
            assignedEntity.Code.CodeSystemName = "NUCC";
            assignedEntity.Code.DisplayName = "Adult Medicine";
            IPN AsName = hl7factory.CreatePN();
            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
            addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
            addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();
            assignedEntity.Addr.Add(addressphno.GenerateAddress(addressinfo, hl7factory));///END

            contactinfo = new PhNoModel();///FIll Clinic Contact Number 
            contactinfo.telcomUse = "WP";
            contactinfo.telcomValue = patientinfo.ptClinicInformation.ClinicPhoneNumber;
            contactinfo.nullFlavor = "UNK";
            assignedEntity.Telecom.Add(addressphno.GeneratePhNo(contactinfo, hl7factory)); ///END

            AsName = assignedEntity.AssignedPerson.Name.Append();///Manage Clinic Name
            nameinfo = new NameModel();
            nameinfo.Createengiven = patientinfo.ptClinicInformation.ClinicName;
            addressphno.FillName(nameinfo, AsName, hl7factory);///FIll Clinic Name  

            legalAuthenticatordetais = clinicalDoc.Xml;
            return legalAuthenticatordetais;
        }
    }
}
