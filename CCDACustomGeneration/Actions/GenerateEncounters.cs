using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GenerateEncounters
    {

        EncounterModel ptEncounter;
        GenerateTableBodyStructure managetable;
        GenerateAddressPhNo addressphno;
        AddressModel addressinfo;
        PhNoModel contactinfo;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocTable tble;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
        ArrayList DataArr = new ArrayList();

        public string FillEncounters(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptEncounter = new EncounterModel();
            CreateComponent(ptEncounter, clinicalDoc, hl7III);// Manage Allergy Component
            FillEncounterContent(patientinfo, hl7III, hl7factory);
            return clinicalDoc.Xml;
        }
        private void FillEncounterContent(PatientClinicalInformation patientinfo, III hl7III, Factory hl7factory)
        {
            if (patientinfo.ptEncounters != null && patientinfo.ptEncounters.Count > 0)
            {
                if (patientinfo.ptEncounters.Count > 0)
                {
                    managetable = new GenerateTableBodyStructure();
                    DataArr.Add("Encounter");
                    DataArr.Add("Performer");
                    DataArr.Add("Location");
                    DataArr.Add("Date");
                    tble = hl7factory.CreateStrucDocTable();
                    thead = tble.Thead;
                    tbody = tble.Tbody.Append();
                    tr = thead.Tr.Append();
                    managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                    //ArrayList alleries = new ArrayList(patientinfo.ptAllergies.ptAllergies);
                    int i = 0;
                    foreach (var item in patientinfo.ptEncounters)
                    {
                        DataArr = new ArrayList();
                        //content = hl7factory.CreateStrucDocContent();
                        //content.XmlId = "encounter" + (i + 1);
                        //content.Items.Add(item.EncounterDescription);
                        DataArr.Add(item.EncounterDescription);
                        DataArr.Add(item.PerformerName);
                        DataArr.Add(patientinfo.ptClinicInformation.ClinicName);
                        DataArr.Add(string.Format("{0:MM/dd/yyyy}", item.EncounterDate));
                        managetable.CreateTableBody(DataArr, hl7factory, tble, tbody, tr);
                        GenerateEncounterEntry(item, patientinfo, i, hl7III, hl7factory);
                        i++;

                    }
                    functionalStatus.Section.Text.Items.Add(tble);
                }
                else
                {
                    CreateEncounterEmptyEntry(hl7III, hl7factory);
                }
            }
			else
			{
				CreateEncounterEmptyEntry(hl7III, hl7factory);
			}
		}
        private void CreateComponent(EncounterModel ptEncounter, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptEncounter.root != null)
            {
                hl7III.Init(ptEncounter.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptEncounter.code != null)
            {
                functionalStatus.Section.Code.Code = ptEncounter.code;
            }

            if (ptEncounter.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptEncounter.codeSystem;
            }

            if (ptEncounter.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptEncounter.codeSystemName;
            }

            if (ptEncounter.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptEncounter.displayName;
            }

            if (ptEncounter.title != null)
            {
                functionalStatus.Section.Title.Text = ptEncounter.title;
            }
        }
        public void GenerateEncounterEntry(Encounters ptEncounters, PatientClinicalInformation patientinfo, int refid, III hl7III, Factory hl7factory)
        {
            var Entry = functionalStatus.Section.Entry.Append();
            Entry.AsEncounter.ClassCode = "ENC";
            Entry.AsEncounter.MoodCode = x_DocumentEncounterMood.EVN;
            hl7III = Entry.AsEncounter.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.49");
            hl7III = Entry.AsEncounter.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Entry.AsEncounter.Code.Code = "99212";
            Entry.AsEncounter.Code.DisplayName = "Outpatient Visit";
            Entry.AsEncounter.Code.CodeSystem = "2.16.840.1.113883.6.12";
            Entry.AsEncounter.Code.CodeSystemName = "CPT";
            IVXB_TS low = new IVXB_TS();
            low.Init(Convert.ToDateTime(ptEncounters.EncounterDate));
            Entry.AsEncounter.EffectiveTime= new IVL_TS().Init(low: low);
            var performer = Entry.AsEncounter.Performer.Append();
            hl7III = performer.AssignedEntity.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            performer.AssignedEntity.Code.Code = "59058001";
            performer.AssignedEntity.Code.CodeSystem = "2.16.840.1.113883.6.96";
            performer.AssignedEntity.Code.CodeSystemName = "SNOMED CT";
            performer.AssignedEntity.Code.DisplayName = "General Physician";
            var Participant = Entry.AsEncounter.Participant.Append();
            Participant.TypeCode = "LOC";
            Participant.ParticipantRole.ClassCode = "SDLOC";
            hl7III = Participant.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.32");
            Participant.ParticipantRole.Code.Code = "1117-1";
            Participant.ParticipantRole.Code.CodeSystem = "2.16.840.1.113883.6.259";
            Participant.ParticipantRole.Code.CodeSystemName = "HealthcareServiceLocation";
            Participant.ParticipantRole.Code.DisplayName = "Family Medicine Clinic";
          
            addressphno = new GenerateAddressPhNo();
            addressinfo = new AddressModel();///Fill Clinic Address
            addressinfo.street = patientinfo.ptClinicInformation.ClinicStreeet;
            addressinfo.city = patientinfo.ptClinicInformation.ClinicCity;
            addressinfo.state = patientinfo.ptClinicInformation.ClinicState;
            addressinfo.country = patientinfo.ptClinicInformation.ClinicCountry;
            addressinfo.pinCode = patientinfo.ptClinicInformation.ClinicZip.ToString();
            Participant.ParticipantRole.Addr.Add(addressphno.GenerateAddress(addressinfo, hl7factory));///END

            contactinfo = new PhNoModel();///FIll Clinic Contact Number 
            contactinfo.telcomUse = "WP";
            contactinfo.telcomValue = patientinfo.ptClinicInformation.ClinicPhoneNumber;
            contactinfo.nullFlavor = "UNK";
            Participant.ParticipantRole.Telecom.Add(addressphno.GeneratePhNo(contactinfo, hl7factory)); ///END

            var Rel = Entry.AsEncounter.EntryRelationship.Append();
            Rel.TypeCode = x_ActRelationshipEntryRelationship.RSON;
            Rel.AsObservation.ClassCode = "OBS";
            Rel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = Rel.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.19");
            hl7III = Rel.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Rel.AsObservation.Code.Code = "404684003";
            Rel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            Rel.AsObservation.Code.CodeSystemName = "SNOMED CT";
            Rel.AsObservation.Code.DisplayName = "Finding";
            Rel.AsObservation.StatusCode.Code = "completed";
            Rel.AsObservation.EffectiveTime= new IVL_TS().Init(low: low); 
            CD obsValueAsCD = new CD();
            obsValueAsCD.Code = ptEncounters.Code;
            obsValueAsCD.DisplayName = Convert.ToString(ptEncounters.EncounterDescription);
            obsValueAsCD.CodeSystem = "2.16.840.1.113883.6.96";
            Rel.AsObservation.Value.Add(obsValueAsCD);
            Rel = Entry.AsEncounter.EntryRelationship.Append();
            Rel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            Rel.AsAct.ClassCode = 0;
            Rel.AsAct.MoodCode = x_DocumentActMood.EVN;
            hl7III = Rel.AsAct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.80");
            hl7III = Rel.AsAct.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Rel.AsAct.Code.Code = "29308-4";
            Rel.AsAct.Code.CodeSystem = "2.16.840.1.113883.6.1";
            Rel.AsAct.Code.CodeSystemName = "LOINC";
            Rel.AsAct.Code.DisplayName = "ENCOUNTER DIAGNOSIS";
            Rel.AsAct.StatusCode.Code = "active";
            Rel.AsAct.EffectiveTime = new IVL_TS().Init(low: low);
            var SubRel = Rel.AsAct.EntryRelationship.Append();
            SubRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            SubRel.AsObservation.ClassCode = "OBS";
            SubRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = SubRel.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.4");
            hl7III = SubRel.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            SubRel.AsObservation.Code.Code = "409586006";
            SubRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            SubRel.AsObservation.Code.DisplayName = "Complaint";
            SubRel.AsObservation.StatusCode.Code = "completed";
            SubRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: low);
            CD obsValueAsCDSub = new CD();
            obsValueAsCDSub.Code = ptEncounters.Code;
            obsValueAsCDSub.DisplayName = Convert.ToString(ptEncounters.EncounterDescription);
            obsValueAsCDSub.CodeSystem = "2.16.840.1.113883.6.96";
            SubRel.AsObservation.Value.Add(obsValueAsCDSub);

        }
        public void CreateEncounterEmptyEntry(III hl7III, Factory hl7factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsAct.ClassCode = x_ActClassDocumentEntryAct.ACT;
            entry.AsAct.MoodCode = x_DocumentActMood.EVN;
            hl7III = entry.AsAct.Id.Append();
            hl7III.NullFlavor = "UNK";
            hl7III = entry.AsAct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.30");
            entry.AsAct.Code.Code = "48765-2";
            entry.AsAct.Code.CodeSystem = "2.16.840.1.113883.6.1";
            entry.AsAct.Code.CodeSystemName = "LOINC";
            entry.AsAct.Code.DisplayName = "Allergies, adverse reactions, alerts";
            entry.AsAct.StatusCode.Code = "completed";
            entry.AsAct.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS(), high: new IVXB_TS());
            // If Status Complete

            IEntryRelationship entryRel = entry.AsAct.EntryRelationship.Append();
            entryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            entryRel.AsObservation.ClassCode = "OBS";
            entryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            III TempId = entryRel.AsObservation.TemplateId.Append();
            TempId.Root = "2.16.840.1.113883.10.20.22.4.7";
            entryRel.AsObservation.StatusCode.Code = "completed";
            hl7III = entryRel.AsObservation.Id.Append();
            hl7III.Root = "1.3.6.1.4.1.22812.11.0.100610.4.10.2";
            hl7III.Extension = "41700060";
            entryRel.AsObservation.Code.Code = "ASSERTION";
            entryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.5.4";
            entryRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
            CD obsValueAsCD = new CD();
            obsValueAsCD.NullFlavor = "UNK";
            entryRel.AsObservation.Value.Add(obsValueAsCD);
            PN pn = new PN();
            pn.Text = "N/A";
            IParticipant2 parti = hl7factory.CreateParticipant2();
            parti.TypeCode = "CSM";
            parti.ParticipantRole.ClassCode = "MANU";
            parti.ParticipantRole.AsPlayingEntity.ClassCode = "MMAT";
            parti.ParticipantRole.AsPlayingEntity.Code.NullFlavor = "UNK";
            parti.ParticipantRole.AsPlayingEntity.Name.Add(pn);
            entryRel.AsObservation.Participant.Add(parti);
        }
    }
}
