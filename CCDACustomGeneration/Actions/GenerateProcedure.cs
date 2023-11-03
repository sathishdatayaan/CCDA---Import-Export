using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GenerateProcedure
    {
        ProcedureModel ptProcedureModel;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        ArrayList DataArr = new ArrayList();
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;

        public string FillPatientProcedure(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptProcedureModel = new ProcedureModel();
            CreateComponent(ptProcedureModel, clinicalDoc, hl7III);// Manage Allergy Component
            //FillProcedureContent(patientinfo, hl7factory);
            int i = 0;

            if (patientinfo.ptProcedure != null && patientinfo.ptProcedure.Count > 0)
            {

                if (patientinfo.ptAllergies.Count > 0)
                {

                    foreach (ProcedureList item in patientinfo.ptProcedure)
                    {
                        GenerateProcedureEntry(item, (i + 1), patientinfo, hl7III, hl7factory);
                    }
                }
                else
                {
                    GenerateProcedureEntryEmpty(hl7III, hl7factory);
                }
            }
			else
			{
				GenerateProcedureEntryEmpty(hl7III, hl7factory);
			}

			return clinicalDoc.Xml;
        }
        private void CreateComponent(ProcedureModel ptProcedureModel, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptProcedureModel.root != null)
            {
                hl7III.Init(ptProcedureModel.root);
            }

            if (ptProcedureModel.code != null)
            {
                functionalStatus.Section.Code.Code = ptProcedureModel.code;
            }

            if (ptProcedureModel.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptProcedureModel.codeSystem;
            }

            if (ptProcedureModel.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptProcedureModel.codeSystemName;
            }

            if (ptProcedureModel.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptProcedureModel.displayName;
            }

            if (ptProcedureModel.title != null)
            {
                functionalStatus.Section.Title.Text = ptProcedureModel.title;
            }
        }
        public void GenerateProcedureEntry(ProcedureList patientProcedure,int refid, PatientClinicalInformation patientinfo, III hl7III, Factory hl7factory)
        {
            IEntry  Entry = functionalStatus.Section.Entry.Append();
            Entry.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            Entry.AsObservation.ClassCode = "OBS";
            hl7III = Entry.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.13");
            hl7III = Entry.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Entry.AsObservation.Code.Code = patientProcedure.CPTCodes;
            Entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            Entry.AsObservation.Code.CodeSystemName = "CPT";
            Entry.AsObservation.Code.DisplayName = patientProcedure.Description;
            Entry.AsObservation.Code.OriginalText.Reference.Value = ("#Proc"+ (refid + 1).ToString());
            Entry.AsObservation.StatusCode.Code = "completed";
            Entry.AsObservation.EffectiveTime.NullFlavor = "UNK";
            Entry.AsObservation.PriorityCode.NullFlavor = "UNK";
            CD obsValueAsCD = new CD();
            obsValueAsCD.OriginalText.Reference.Value = ("#Proc" + (refid + 1).ToString());
            Entry.AsObservation.Value.Add(obsValueAsCD);
            if (patientinfo.ptClinicInformation.ClinicName!=null)
            {
                addressphno = new GenerateAddressPhNo();
                addressinfo = new AddressModel();///Fill Clinic Address
                addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
                addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
                addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
                addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
                addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();
                contactinfo = new PhNoModel();///FIll Clinic Contact Number 
                contactinfo.telcomUse = "WP";
                contactinfo.telcomValue = patientinfo.ptClinicInformation.ClinicPhoneNumber;
                contactinfo.nullFlavor = "UNK";
            }
            var Performer = Entry.AsObservation.Performer.Append();
            hl7III = Performer.AssignedEntity.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Performer.AssignedEntity.Addr.Add(addressphno.GenerateAddress(addressinfo,hl7factory));
            Performer.AssignedEntity.Telecom.Add(addressphno.GeneratePhNo(contactinfo,hl7factory));
            hl7III = Performer.AssignedEntity.RepresentedOrganization.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            IAD hl7IADInterface = hl7factory.CreateAD();
            hl7IADInterface.NullFlavor = "UNK";
            Performer.AssignedEntity.RepresentedOrganization.Addr.Add(hl7IADInterface);
            ITEL telcom = hl7factory.CreateTEL();
            telcom = hl7factory.CreateTEL();
            telcom.NullFlavor = "UNK";
            Performer.AssignedEntity.RepresentedOrganization.Telecom.Add(telcom);
        }
        public void GenerateProcedureEntryEmpty(III hl7III, Factory hl7factory)
        {
            var EntryN = functionalStatus.Section.Entry.Append();
            EntryN.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            EntryN.AsObservation.ClassCode = "OBS";
            hl7III = EntryN.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.13");
            hl7III = EntryN.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            EntryN.AsObservation.Code.NullFlavor = "UNK";
            EntryN.AsObservation.StatusCode.NullFlavor = "UNK";
            EntryN.AsObservation.EffectiveTime.NullFlavor = "UNK";
            EntryN.AsObservation.PriorityCode.NullFlavor = "UNK";
            CD ValueAsCD = new CD();
            EntryN.AsObservation.Value.Add(ValueAsCD);
        }
    }
}
