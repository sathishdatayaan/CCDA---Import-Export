using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateParticipantInfo
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;

        public string FillParticipantInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string participantdetais = string.Empty;
            var participant = clinicalDoc.Participant.Append();
            var times = hl7factory.CreateIVXB_TS();
            times.Init(DateTime.Now);
            IPN AsName = hl7factory.CreatePN();
            participant.TypeCode = "IND";
            participant.Time.Init(null, times, times);
            participant.AssociatedEntity.ClassCode = "CAREGIVER";
            participant.AssociatedEntity.Code.Code = "MTH";
            participant.AssociatedEntity.Code.CodeSystem = "2.16.840.1.113883.5.111";
            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
            addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
            addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();
            participant.AssociatedEntity.Addr.Add(addressphno.GenerateAddress(addressinfo, hl7factory));///END

            contactinfo = new PhNoModel();///FIll Clinic Contact Number 
            contactinfo.telcomUse = "WP";
            contactinfo.telcomValue = patientinfo.ptClinicInformation.ClinicPhoneNumber;
            contactinfo.nullFlavor = "UNK";
            participant.AssociatedEntity.Telecom.Add(addressphno.GeneratePhNo(contactinfo, hl7factory)); ///END

            AsName = participant.AssociatedEntity.AssociatedPerson.Name.Append();///Manage Clinic Name
            nameinfo = new NameModel();
            nameinfo.Createengiven = patientinfo.ptClinicInformation.ClinicName;
            addressphno.FillName(nameinfo, AsName, hl7factory);///FIll Clinic Name  

            participantdetais = clinicalDoc.Xml;
            return participantdetais;
        }
    }
}
