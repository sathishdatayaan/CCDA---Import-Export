using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateCustodian
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        public string FillCustodianInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string custodianInfo = string.Empty;
            var assignedCustodian = clinicalDoc.Custodian.AssignedCustodian.RepresentedCustodianOrganization;
            hl7III = assignedCustodian.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6");
            var CName = hl7factory.CreateON();
            CName.Text = patientinfo.ptClinicInformation.ClinicName;
            assignedCustodian.Name = CName;
           
            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
            addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
            addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();
            GenerateAddress(addressinfo, assignedCustodian, hl7factory);///END

            contactinfo = new PhNoModel();///FIll Clinic Contact Number 
            contactinfo.telcomUse = "WP";
            contactinfo.telcomValue = patientinfo.ptClinicInformation.ClinicPhoneNumber;
            contactinfo.nullFlavor = "UNK";
            GeneratePhNo(contactinfo, assignedCustodian, hl7factory); ///END

            custodianInfo = clinicalDoc.Xml;
            return custodianInfo;
        }

        private void GeneratePhNo(PhNoModel contactinfo, ICustodianOrganization assignedCustodian, Factory hl7factory)
        {
            if(contactinfo.telcomValue!="" && contactinfo.telcomValue != null)
            {
                var telcom = hl7factory.CreateTEL();
                telcom.Use =contactinfo.telcomUse;
                telcom.Value = contactinfo.telcomValue;
                assignedCustodian.Telecom = telcom;
            }
        }

        private void GenerateAddress(AddressModel addressinfo, ICustodianOrganization assignedCustodian, Factory hl7factory)
        {
            var Street = hl7factory.CreateadxpstreetAddressLine();
            var City = hl7factory.Createadxpcity();
            var State = hl7factory.Createadxpstate();
            var Country = hl7factory.Createadxpcountry();
            var PotalCd = hl7factory.CreateadxppostalCode();
            if (addressinfo.street != "" && addressinfo.street != null)
            {
                Street.Init(addressinfo.street);
                assignedCustodian.Addr.Items.Add(Street);
            }
            if (addressinfo.city != "" && addressinfo.city != null)
            {
                City.Init(addressinfo.city);
                assignedCustodian.Addr.Items.Add(City);
            }
            if (addressinfo.state != "" && addressinfo.state != null)
            {
                State.Init(addressinfo.state);
                assignedCustodian.Addr.Items.Add(State);
            }
            if (addressinfo.country != "" && addressinfo.country != null)
            {
                Country.Init(addressinfo.country);
                assignedCustodian.Addr.Items.Add(Country);
            }
            if (addressinfo.pinCode != "" && addressinfo.pinCode != null)
            {
                PotalCd.Init(addressinfo.pinCode);
            }
            else
            {
                PotalCd.NullFlavor = addressinfo.nullFlavor;
            }
        }
    }
}
