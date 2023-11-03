using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Actions
{
    public class GenerateDocumentationOf
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;


        public string FillDocumentationOf(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string documentationOfdetais = string.Empty;

            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
            addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
            addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();

            contactinfo = new PhNoModel();///FIll Clinic Contact Number 
            contactinfo.telcomUse = "WP";
            //contactinfo.telcomValue = patientinfo.ClinicPhoneNumber;
            contactinfo.nullFlavor = "UNK";

           

            var docof = clinicalDoc.DocumentationOf.Append();
            int count = 0;
            foreach (DocumentationOfList item in patientinfo.documentationOfInfo)
            {
                IVXB_TS low = new IVXB_TS();
                docof.ServiceEvent.ClassCode = "PCPR";
                docof.ServiceEvent.Code.Code = "99214";
                docof.ServiceEvent.Code.CodeSystem = "2.16.840.1.113883.6.12";
                docof.ServiceEvent.Code.CodeSystemName = "CPT4";
                docof.ServiceEvent.Code.DisplayName = "Office Visit";
                var performer = docof.ServiceEvent.Performer.Append();
                
                performer.TypeCode = x_ServiceEventPerformer.PRF;
                if (count == 0)
                {
                    low.Init(Convert.ToDateTime(item.date));
                    docof.ServiceEvent.EffectiveTime = new IVL_TS().Init(low: low);
                    performer.FunctionCode.Code = "PP";
                }
                else
                {
                    if (Convert.ToDateTime(item.date).Date < Convert.ToDateTime(patientinfo.documentationOfInfo[count - 1].date).Date)
                    {
                        low.Init(Convert.ToDateTime(item.date));
                        docof.ServiceEvent.EffectiveTime = new IVL_TS().Init(low: low);
                    }
                    performer.FunctionCode.Code = "SP";
                }
                performer.FunctionCode.DisplayName = "Care Provider";
                performer.FunctionCode.CodeSystem = "2.16.840.1.113883.12.443";
                performer.FunctionCode.CodeSystemName = "Provider Role";
                low.Init(Convert.ToDateTime(item.date));
                performer.Time = new IVL_TS().Init(low: low); 
                var id = performer.AssignedEntity.Id.Append();
                id.Init("2.16.840.1.113883.4.6", "111111111", "NPI");
                performer.AssignedEntity.Addr.Add(addressphno.GenerateAddress(addressinfo, hl7factory));
                contactinfo.telcomValue = "mailto: info @drummondgroup.com";
                performer.AssignedEntity.Telecom.Add(addressphno.GeneratePhNo(contactinfo, hl7factory));
                IPN AsName = hl7factory.CreatePN();
                AsName = performer.AssignedEntity.AssignedPerson.Name.Append();///Manage Clinic Name
                nameinfo = new NameModel();
                //nameinfo.Createengiven = patientinfo.ClinicName;
                nameinfo.Createenfamily = item.staffName;
                addressphno.FillName(nameinfo, AsName, hl7factory);///FIll Clinic Name  
                count++;
            }
            documentationOfdetais = clinicalDoc.Xml;
            return documentationOfdetais;
        }
    }
}
