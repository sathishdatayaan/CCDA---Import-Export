using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateAddressPhNo 
    {
        public ITEL GeneratePhNo(PhNoModel telcominfo, Factory hl7factory)
        {
            ITEL telcom = hl7factory.CreateTEL();
            telcom.Use = telcominfo.telcomUse;
            if (telcominfo.telcomValue != "" && telcominfo.telcomValue != null)
            {
                telcom.Value = telcominfo.telcomValue;
            }
            else
            {
                telcom.NullFlavor = telcominfo.nullFlavor;
            }
            return telcom;
        }

        public string GetDateWithFormat(string dateString)
        {
            string patientDOB = string.Empty;
            patientDOB = Convert.ToDateTime(dateString).Year.ToString();
            if (Convert.ToDateTime(dateString).Month.ToString().Length == 1)
            {
                patientDOB = patientDOB + "0" + Convert.ToDateTime(dateString).Month;
            }
            else
            {
                patientDOB = patientDOB + Convert.ToDateTime(dateString).Month;
            }
            if (Convert.ToDateTime(dateString).Day.ToString().Length == 1)
            {
                patientDOB = patientDOB + "0" + Convert.ToDateTime(dateString).Day;
            }
            else
            {
                patientDOB = patientDOB + Convert.ToDateTime(dateString).Day;
            }
            return patientDOB;
        }

        public IAD GenerateAddress(AddressModel addressinfo, Factory hl7factory)
        {
            IAD hl7IADInterface = hl7factory.CreateAD();
            if (addressinfo.street != "" && addressinfo.street != null)
            {
                var Street = hl7factory.CreateadxpstreetAddressLine();
                Street.Init(addressinfo.street);
                hl7IADInterface.Items.Add(Street);
            }
            if (addressinfo.city != "" && addressinfo.city != null)
            {
                var City = hl7factory.Createadxpcity();
                City.Init(addressinfo.city);
                hl7IADInterface.Items.Add(City);
            }
            if (addressinfo.state != "" && addressinfo.state != null)
            {
                var State = hl7factory.Createadxpstate();
                State.Init(addressinfo.state);
                hl7IADInterface.Items.Add(State);
            }
            if (addressinfo.country != "" && addressinfo.country != null)
            {
                var Country = hl7factory.Createadxpcountry();
                Country.Init(addressinfo.country);
                hl7IADInterface.Items.Add(Country);
            }
            var PostalCode = hl7factory.CreateadxppostalCode();
            if (addressinfo.pinCode != "" && addressinfo.pinCode != null)
            {
                PostalCode.Init(addressinfo.pinCode);
            }
            else
            {
                PostalCode.NullFlavor = addressinfo.nullFlavor;
            }
            hl7IADInterface.Items.Add(PostalCode);
            return hl7IADInterface;
        }

        public void FillName(NameModel nameinfo, IPN Name, Factory hl7factory)
        {
            if (nameinfo.Createengiven != "" && nameinfo.Createengiven != null)
            {
                var Given = hl7factory.Createengiven();
                Given.Init(nameinfo.Createengiven);
                Name.Items.Add(Given);
            }
            if (nameinfo.Createenfamily != "" && nameinfo.Createenfamily != null)
            {
                var Family = hl7factory.Createenfamily();
                Family.Init(nameinfo.Createenfamily);
                Name.Items.Add(Family);
            }
            if (nameinfo.CreateenSuffix != "" && nameinfo.CreateenSuffix != null)
            {
                var Family = hl7factory.Createenprefix();
                Family.Init(nameinfo.CreateenSuffix);
                Name.Items.Add(Family);
            }
        }
    }
}
