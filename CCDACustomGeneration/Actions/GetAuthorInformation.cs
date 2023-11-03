using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GetAuthorInformation
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;

        public string FillAuthorInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string authordetais = string.Empty;
            //var targetrole = clinicalDoc.RecordTarget.Append();
            var authors = clinicalDoc.Author.Append();
            authors.Time.AsDateTime = DateTime.Now;
            var assignedAuthor = authors.AssignedAuthor;
            hl7III = assignedAuthor.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6", "999999");
            assignedAuthor.Code.Code = "200000000X";
            assignedAuthor.Code.CodeSystem = "2.16.840.1.113883.6.101";
            assignedAuthor.Code.CodeSystemName = "NUCC";
            assignedAuthor.Code.DisplayName = "Allopathic &amp; Osteopathic Physicians";
            IPN AsName = hl7factory.CreatePN();
            IENXP Ienxn = hl7factory.CreateENXP();
            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
            addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
            addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();
            assignedAuthor.Addr.Add(addressphno.GenerateAddress(addressinfo, hl7factory));///END

            contactinfo = new PhNoModel();///FIll Clinic Contact Number 
            contactinfo.telcomUse = "WP";
            contactinfo.telcomValue = patientinfo.ptClinicInformation.ClinicPhoneNumber;
            contactinfo.nullFlavor = "UNK";
            assignedAuthor.Telecom.Add(addressphno.GeneratePhNo(contactinfo, hl7factory)); ///END

            AsName = assignedAuthor.AsPerson.Name.Append();///Manage Clinic Name
            nameinfo = new NameModel();
            nameinfo.Createengiven = patientinfo.ptClinicInformation.ClinicName;
            //nameinfo.Createenfamily = patientinfo.LastName;
            addressphno.FillName(nameinfo, AsName, hl7factory);///FIll Clinic Name  

            authordetais = clinicalDoc.Xml;
            return authordetais;
        }
    }
}
