using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateRecipientInfo
    {
        GenerateAddressPhNo addressphno;
        NameModel nameinfo;

        public string FillRecipientInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string recipientInfodetais = string.Empty;
            var informationRecipient = clinicalDoc.InformationRecipient.Append();
            IPN AsName = hl7factory.CreatePN();
            AsName = informationRecipient.IntendedRecipient.InformationRecipient.Name.Append();///Manage Clinic Name
            addressphno = new GenerateAddressPhNo();
            nameinfo = new NameModel();
            nameinfo.Createengiven = patientinfo.ptClinicInformation.ClinicName;
            addressphno.FillName(nameinfo, AsName, hl7factory);///FIll Clinic Name  
            var CName = hl7factory.CreateON();
            CName.Text = patientinfo.ptClinicInformation.ClinicName;
            informationRecipient.IntendedRecipient.ReceivedOrganization.Name.Add(CName);
            recipientInfodetais = clinicalDoc.Xml;
            return recipientInfodetais;
        }
    }
}
