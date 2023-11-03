using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateDataEnterer
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;


        public string FillDataEnterer(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string dataentererdetais = string.Empty;
            var assignedEntity = clinicalDoc.DataEnterer.AssignedEntity;
            //assignedEntity.Time.AsDateTime = DateTime.Now;
            //var assignedAuthor = assignedEntity.AssignedAuthor;
            hl7III = assignedEntity.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6", "999999943252");
            assignedEntity.Code.Code = "200000000X";
            assignedEntity.Code.CodeSystem = "2.16.840.1.113883.6.101";
            assignedEntity.Code.CodeSystemName = "NUCC";
            assignedEntity.Code.DisplayName = "Allopathic &amp; Osteopathic Physicians";
            IPN AsName = hl7factory.CreatePN();
            IENXP Ienxn = hl7factory.CreateENXP();
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
            //nameinfo.Createenfamily = patientinfo.LastName;
            addressphno.FillName(nameinfo, AsName, hl7factory);///FIll Clinic Name  

            dataentererdetais = clinicalDoc.Xml;
            return dataentererdetais;
        }
    }
}
