﻿using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateAuthenticator
    {
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        NameModel nameinfo;      

        public string FillLegalAuthenticatorInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string authenticatordetais = string.Empty;
            var authenticator = clinicalDoc.Authenticator.Append();
            authenticator.Time.Init(DateTime.Now);
            authenticator.SignatureCode.Init("S");
            var assignedEntity = authenticator.AssignedEntity;
            hl7III = assignedEntity.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6");
            IPN AsName = hl7factory.CreatePN();
            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street =  patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city =    patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state =   patientinfo.ptClinicInformation.ClinicState;
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

            authenticatordetais = clinicalDoc.Xml;
            return authenticatordetais;
        }
    }
}
